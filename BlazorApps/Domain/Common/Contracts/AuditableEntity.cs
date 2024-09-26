namespace Domain.Common.Contracts
{
    public abstract class AuditableEntity : AuditableEntity<DefaultIdType>
    {
    }

    public abstract class AuditableEntity<T> : BaseEntity<T>, IAuditableEntity, ISoftDelete
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; private set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }

        protected AuditableEntity()
        {
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
        }
    }

    public abstract class AuditableEntityWithIntKey : AuditableEntity<int>
    {
    }

    public abstract class AuditableEntityWithIntKey<T> : BaseEntity<T>, IAuditableEntity, ISoftDelete
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; private set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }

        protected AuditableEntityWithIntKey()
        {
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
        }
    }
    public abstract class MasterDataAuditableEntityWithIntKey : MasterDataAuditableEntityWithIntKey<int>
    {
    }

    public abstract class MasterDataAuditableEntityWithIntKey<T> : BaseEntity<T>
    {

    }
}