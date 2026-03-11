namespace UD.Core.Helper.Configuration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public interface IBaseEntity
    {
        object[] GetKeys();
    }
    public interface IBaseEntity<TKey> : IBaseEntity
    {
        TKey Id { get; set; }
    }
    [Serializable]
    public abstract class BaseEntity : IBaseEntity
    {
        public override string ToString() => $"[ENTITY: {this.GetType().Name}], Keys = {String.Join(", ", this.GetKeys())}";
        public abstract object[] GetKeys();
    }
    [Serializable]
    public abstract class BaseEntity<TKey> : BaseEntity, IBaseEntity<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }
        protected BaseEntity() : this(default) { }
        protected BaseEntity(TKey Id)
        {
            this.Id = Id;
        }
        public override object[] GetKeys() => new object[] { this.Id };
        public override string ToString() => $"[ENTITY: {this.GetType().Name}], {nameof(this.Id)} = {this.Id}";
    }
}