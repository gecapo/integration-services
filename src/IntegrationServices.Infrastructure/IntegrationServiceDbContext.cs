using Microsoft.EntityFrameworkCore;

namespace IntegrationServices.Infrastructure
{
    public class IntegrationServiceDbContext : DbContext
    {

        public IntegrationServiceDbContext(DbContextOptions<IntegrationServiceDbContext> options) : base(options)
        {
        }

        public DbSet<Audit> Audits { get; set; }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var audit = await OnBeforeSaveChangesAsync();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IntegrationServiceDbContext).Assembly);
        }

        private async Task<List<AuditDto>> OnBeforeSaveChangesAsync()
        {
            ChangeTracker.DetectChanges();

            var auditEntries = new List<AuditDto>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit
                    || entry.State == EntityState.Detached
                    || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditDto(entry);
                auditEntry.TableName = entry.Metadata.GetSchemaQualifiedTableName();
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    var propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                                try
                                {
                                    var oldValueA = await entry.GetDatabaseValuesAsync();

                                    var oldValue = oldValueA?.GetValue<object>(propertyName) ?? null;
                                    var newValue = property?.CurrentValue ?? null;
                                    if (oldValue == null && newValue != null ||
                                        oldValue != null && !oldValue.Equals(newValue))
                                    {
                                        auditEntry.OldValues[propertyName] = oldValue;
                                        auditEntry.NewValues[propertyName] = newValue;
                                    }
                                }
                                catch (Exception e){}
                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
                Audits.Add(auditEntry);

            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private async Task<int> OnAfterSaveChangesAsync(List<AuditDto> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return await Task.FromResult(0);

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                    if (prop.Metadata.IsPrimaryKey())
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    else
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;

                Audits.Add(auditEntry);
            }

            return await base.SaveChangesAsync();
        }
    }
}
