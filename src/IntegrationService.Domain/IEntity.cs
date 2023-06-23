using System;
using System.ComponentModel.DataAnnotations;

namespace IntegrationServices.Domain
{
    public interface IEntity
    {
        object Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
