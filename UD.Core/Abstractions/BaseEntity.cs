using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UD.Core.Abstractions
{
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
        public override string ToString() => $"ENTITY: {this.GetType().FullName}, [Keys] = [{String.Join(", ", this.GetKeys())}]";
        public abstract object[] GetKeys();
    }
    [Serializable]
    public abstract class BaseEntity<TKey> : BaseEntity, IBaseEntity<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }
        protected BaseEntity() : this(default) { }
        protected BaseEntity(TKey id)
        {
            this.Id = id;
        }
        public override object[] GetKeys() => [this.Id];
        public override string ToString() => $"ENTITY: {this.GetType().FullName}, [{nameof(this.Id)}] = {this.Id}";
    }
}