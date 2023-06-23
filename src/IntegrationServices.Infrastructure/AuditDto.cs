using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IntegrationServices.Infrastructure
{
    internal class AuditDto
    {
        private EntityEntry entry;

        public AuditDto(EntityEntry entry)
        {
            this.entry = entry;
        }

        public string TableName { get; internal set; }
        public Dictionary<string, object> KeyValues { get; internal set; }
        public Dictionary<string, object> OldValues { get; internal set; }
        public Dictionary<string,object> NewValues { get; internal set; }
        public bool HasTemporaryProperties { get; internal set; }
        public ICollection<PropertyEntry> TemporaryProperties { get; internal set; }
    }
}