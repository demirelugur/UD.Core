namespace UD.Core.Helper.Database
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Linq;
    using UD.Core.Extensions;
    public sealed class ChangeEntry
    {
        public Dictionary<string, object> entity { get; set; }
        public string entityName { get; set; }
        public EntityState entityState { get; set; }
        public Dictionary<string, ChangePropertyInfo> changeProperties { get; set; }
        public ChangeEntry() : this(default, default, default, default) { }
        public ChangeEntry(EntityEntry entry, Dictionary<string, ChangePropertyInfo> changeProperties) : this(entry.Entity, entry.Metadata.ClrType, entry.State, changeProperties) { }
        public ChangeEntry(object entity, Type entityType, EntityState entityState, Dictionary<string, ChangePropertyInfo> changeProperties)
        {
            this.entity = ExtractScalarProperties(entity, entityType);
            this.entityName = entityType.GetTableName(true);
            this.entityState = entityState;
            this.changeProperties = changeProperties ?? [];
        }
        private static Dictionary<string, object> ExtractScalarProperties(object entity, Type entityType)
        {
            if (entity == null) { return []; }
            return entityType.GetProperties().Where(prop => prop.IsMapped()).ToDictionary(prop => prop.GetColumnName(), prop => maskHTML(prop.GetValue(entity)));
        }
        internal static object maskHTML(object value)
        {
            if (value is String _s && ValidationChecks.IsHtml(_s)) { return "[HTML content]"; }
            return value;
        }
    }
}