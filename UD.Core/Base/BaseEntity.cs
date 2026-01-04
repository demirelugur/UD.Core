namespace FaturaBilgileri.DAL.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public interface IEntity
    {
        object[] GetKeys();
    }
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }
    [Serializable]
    public abstract class BaseEntity : IEntity
    {
        public override string ToString() => $"[ENTITY: {this.GetType().Name}] Keys = {String.Join(", ", this.GetKeys())}";
        public abstract object[] GetKeys();
    }
    [Serializable]
    public abstract class BaseEntity<TKey> : BaseEntity, IEntity<TKey>
    {
        private TKey _id;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get { return _id; } set { _id = value; } }
        protected BaseEntity() : this(default) { }
        protected BaseEntity(TKey Id)
        {
            this.Id = Id;
        }
        public override object[] GetKeys() => new object[] { this.Id };
        public override string ToString() => $"[ENTITY: {this.GetType().Name}] Id = {this.Id}";
    }
}