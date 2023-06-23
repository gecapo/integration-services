using IntegrationServices.Domain;

namespace IntegrationServices.Domain
{
    public abstract class BaseEntity<T> : IEntity
    {
        public T Id { get; set; }
        object IEntity.Id
        {
            get { return Id; }
            set { }
        }

        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }
    }

}

public class Audit : BaseEntity<int>
{
    public string? TableName { get; set; }
    public string? KeyValues { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
}

