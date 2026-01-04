namespace UD.Core.Auditing
{
    using FaturaBilgileri.DAL.Entities;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; set; }
    }
    public interface IMayHaveCreator<AuditKey> : IHasCreationTime where AuditKey : struct
    {
        AuditKey CreatorId { get; set; }
        void SetCreator(AuditKey creatorId);
    }
    public interface ICreationAuditedObject<AuditKey> : IMayHaveCreator<AuditKey> where AuditKey : struct { }
    [Serializable]
    public abstract class CreationAuditedEntity<TKey, AuditKey> : BaseEntity<TKey>, ICreationAuditedObject<AuditKey> where AuditKey : struct
    {
        private DateTime _creationTime;
        private AuditKey _creatorId;
        [Column(TypeName = "datetime")]
        public virtual DateTime CreationTime { get { return _creationTime; } set { _creationTime = value; } }
        public virtual AuditKey CreatorId { get { return _creatorId; } set { _creatorId = value; } }
        public void SetCreator(AuditKey creatorId)
        {
            this.CreationTime = DateTime.UtcNow;
            this.CreatorId = creatorId;
        }
        protected CreationAuditedEntity() : this(default, default, default) { }
        protected CreationAuditedEntity(DateTime creationTime, AuditKey creatorId, TKey id) : base(id)
        {
            this.CreationTime = creationTime;
            this.CreatorId = creatorId;
        }
    }
}