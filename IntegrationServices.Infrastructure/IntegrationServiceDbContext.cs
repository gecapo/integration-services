using Microsoft.EntityFrameworkCore;

namespace IntegrationServices.Infrastructure
{
    public class IntegrationServiceDbContext : DbContext
    {

        public IntegrationServiceDbContext(DbContextOptions<IntegrationServiceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IntegrationServiceDbContext).Assembly);
        }
    }
}