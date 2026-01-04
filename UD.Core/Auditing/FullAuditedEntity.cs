namespace UD.Core.Auditing
{
    using System.ComponentModel.DataAnnotations.Schema;
    using UD.Core.Extensions;
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
    public interface IHasDeletionTime : ISoftDelete
    {
        DateTime? DeletionTime { get; set; }
    }
    public interface IDeletionAuditedObject<AuditKey> : IHasDeletionTime where AuditKey : struct
    {
        AuditKey? DeleterId { get; set; }
        void SetDeleter(AuditKey deleterId);
    }
    public interface IFullAuditedObject<AuditKey> : IAuditedObject<AuditKey>, IDeletionAuditedObject<AuditKey> where AuditKey : struct { }
    [Serializable]
    public abstract class FullAuditedEntity<TKey, AuditKey> : AuditedEntity<TKey, AuditKey>, IFullAuditedObject<AuditKey> where AuditKey : struct
    {
        private bool _isDeleted;
        private DateTime? _deletionTime;
        private AuditKey? _deleterId;
        public virtual bool IsDeleted { get { return _isDeleted; } set { _isDeleted = value; } }
        [Column(TypeName = "datetime")]
        public virtual DateTime? DeletionTime { get { return _deletionTime; } set { _deletionTime = value.NullOrDefault(); } }
        public virtual AuditKey? DeleterId { get { return _deleterId; } set { _deleterId = value.NullOrDefault(); } }
        public void SetDeleter(AuditKey deleterId)
        {
            this.IsDeleted = true;
            this.DeletionTime = DateTime.UtcNow;
            this.DeleterId = deleterId;
        }
        protected FullAuditedEntity() : this(default, default, default, default, default, default, default, default) { }
        protected FullAuditedEntity(bool isDeleted, DateTime? deletionTime, AuditKey? deleterId, DateTime? lastModificationTime, AuditKey? lastModifierId, DateTime creationTime, AuditKey creatorId, TKey id) : base(lastModificationTime, lastModifierId, creationTime, creatorId, id)
        {
            this.IsDeleted = isDeleted;
            this.DeletionTime = deletionTime;
            this.DeleterId = deleterId;
        }
    }
}