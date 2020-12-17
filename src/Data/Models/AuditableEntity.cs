using System;
// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

namespace BlitzFramework.Data.Models
{
    public class AuditableEntity<T> : Entity<T>
    {
        public T CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
        public T UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}