namespace UD.Core.Helper.Database
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using UD.Core.Extensions;
    public sealed class ChangeEntry
    {
        public object entity { get; set; }
        public string entityName { get; set; }
        public string entityState { get; set; }
        public Dictionary<string, ChangePropertyInfo> changeProperties { get; set; }
        public ChangeEntry() : this(default, default, default, default) { }
        public ChangeEntry(EntityEntry entry, Dictionary<string, ChangePropertyInfo> changeProperties) : this(entry.Entity, entry.Metadata.ClrType, entry.State, changeProperties) { }
        public ChangeEntry(object entity, Type entityType, EntityState entityState, Dictionary<string, ChangePropertyInfo> changeProperties)
        {
            this.entity = entity;
            this.entityName = entityType.GetTableName(true);
            this.entityState = entityState.ToString("g");
            this.changeProperties = changeProperties ?? [];
        }
    }
}