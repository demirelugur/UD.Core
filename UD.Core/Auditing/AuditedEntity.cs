namespace UD.Core.Auditing
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using UD.Core.Extensions;
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
    public interface IModificationAuditedObject<AuditKey> : IHasModificationTime where AuditKey : struct
    {
        AuditKey? LastModifierId { get; set; }
        void SetLastModifier(AuditKey lastModifierId);
    }
    public interface IAuditedObject<AuditKey> : ICreationAuditedObject<AuditKey>, IModificationAuditedObject<AuditKey> where AuditKey : struct { }
    [Serializable]
    public abstract class AuditedEntity<TKey, AuditKey> : CreationAuditedEntity<TKey, AuditKey>, IAuditedObject<AuditKey> where AuditKey : struct
    {
        private DateTime? _lastModificationTime;
        private AuditKey? _lastModifierId;
        [Column(TypeName = "datetime")]
        public virtual DateTime? LastModificationTime { get { return _lastModificationTime; } set { _lastModificationTime = value.NullOrDefault(); } }
        public virtual AuditKey? LastModifierId { get { return _lastModifierId; } set { _lastModifierId = value.NullOrDefault(); } }
        public void SetLastModifier(AuditKey lastModifierId)
        {
            this.LastModificationTime = DateTime.UtcNow;
            this.LastModifierId = lastModifierId;
        }
        protected AuditedEntity() : this(default, default, default, default, default) { }
        protected AuditedEntity(DateTime? lastModificationTime, AuditKey? lastModifierId, DateTime creationTime, AuditKey creatorId, TKey id) : base(creationTime, creatorId, id)
        {
            this.LastModificationTime = lastModificationTime;
            this.LastModifierId = lastModifierId;
        }
    }
}