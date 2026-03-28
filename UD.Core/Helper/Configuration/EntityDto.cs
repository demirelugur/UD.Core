namespace UD.Core.Helper.Configuration
{
    using System;
    public interface IEntityDto { }
    public interface IEntityDto<TKey> : IEntityDto
    {
        TKey Id { get; set; }
    }
    [Serializable]
    public abstract class EntityDto : IEntityDto
    {
        public override string ToString() => $"[DTO: {this.GetType().Name}]";
    }
    [Serializable]
    public abstract class EntityDto<TKey> : EntityDto, IEntityDto<TKey>
    {
        public TKey Id { get; set; }
        public override string ToString() => $"{base.ToString()}, [{nameof(this.Id)}] = {this.Id}";
    }
}