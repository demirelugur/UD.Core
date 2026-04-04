namespace UD.Core.Helper.Database
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Linq;
    using UD.Core.Attributes;
    using UD.Core.Extensions;
    public sealed class ChangeEntry
    {
        public string entityName { get; set; }
        public Dictionary<string, object> entity { get; set; }
        public Dictionary<string, ChangePropertyInfo> changeProperties { get; set; }
        public ChangeEntry() : this(default, default, default) { }
        public ChangeEntry(EntityEntry entry, Dictionary<string, ChangePropertyInfo> changeProperties) : this(entry.Metadata.ClrType, entry.Entity, changeProperties) { }
        public ChangeEntry(Type entityType, object entity, Dictionary<string, ChangePropertyInfo> changeProperties)
        {
            this.entityName = entityType.GetTableName(true);
            this.entity = extractScalarProperties(entity, entityType);
            this.changeProperties = changeProperties ?? [];
        }
        private static Dictionary<string, object> extractScalarProperties(object entity, Type entityType)
        {
            if (entity == null) { return []; }
            return entityType.GetProperties().Where(prop => prop.IsMapped()).ToDictionary(prop => prop.GetColumnName(), prop => (prop.IsHtmlContent() ? HtmlContentAttribute.title : prop.GetValue(entity)));
        }
    }
}