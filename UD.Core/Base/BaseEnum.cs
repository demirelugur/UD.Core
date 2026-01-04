namespace UD.Core.Base
{
    using FaturaBilgileri.DAL.Entities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using UD.Core.Auditing;
    using UD.Core.Extensions;
    public interface IBaseEnum : IEntity
    {
        string Name { get; set; }
        string? Code { get; set; }
        string? Description { get; set; }
    }
    public abstract class BaseEnum<TKey, TEnum> : BaseEnum<TKey> where TKey : struct where TEnum : Enum
    {
        [NotMapped]
        public TEnum Not_Value
        {
            get { return base.ToEnum<TEnum>(); }
            set { this.Id = value.ToLong().ToString().ParseOrDefault<TKey>(); }
        }
    }
    public abstract class BaseEnum<TKey> : BaseEntity<TKey>, IBaseEnum, ISoftDelete where TKey : struct
    {
        public const int Name_maxlength = 100;
        public const int Code_maxlength = 5;
        public const int Description_maxlength = 255;
        private TKey _id;
        private string _name;
        private string? _code;
        private string? _description;
        private bool _isDeleted;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override TKey Id { get { return _id; } set { _id = value; } }
        [Required]
        [MaxLength(Name_maxlength)]
        [Column(TypeName = "varchar(100)")]
        public virtual string Name { get { return _name; } set { _name = value.ToStringOrEmpty(); } }
        [MaxLength(Code_maxlength)]
        [Column(TypeName = "varchar(5)")]
        public virtual string? Code { get { return _code; } set { _code = value.ParseOrDefault<string>(); } }
        [MaxLength(Description_maxlength)]
        [Column(TypeName = "varchar(255)")]
        public virtual string? Description { get { return _description; } set { _description = value.ParseOrDefault<string>(); } }
        public bool IsDeleted { get { return _isDeleted; } set { _isDeleted = value; } }
        public override string ToString() => this.Name;
        public virtual T ToEnum<T>() where T : Enum => (T)Enum.ToObject(typeof(T), this.Id);
    }
}