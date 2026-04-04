namespace UD.Core.Helper.Database
{
    using Microsoft.AspNetCore.Http;
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
        public ChangeEntry(string entityName, Dictionary<string, object> entity, Dictionary<string, ChangePropertyInfo> changeProperties)
        {
            this.entityName = entityName ?? "";
            this.entity = entity ?? [];
            this.changeProperties = changeProperties ?? [];
        }
        /// <summary><paramref name="value"/> için tanımlanan nesneler: ChangeEntry, EntityEntry, IFormCollection, AnonymousObjectClass</summary>
        public static ChangeEntry ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is ChangeEntry _ce) { return _ce; }
            if (value is EntityEntry _ee)
            {
                Dictionary<string, ChangePropertyInfo> changes = null;
                if (_ee.State == EntityState.Modified)
                {
                    changes = _ee.OriginalValues.Properties
                    .Where(prop => prop.PropertyInfo.IsMapped())
                    .Select(prop => new
                    {
                        Property = prop,
                        Original = _ee.OriginalValues[prop],
                        Current = _ee.CurrentValues[prop]
                    })
                    .Where(x => !Equals(x.Original, x.Current))
                    .ToDictionary(
                        prop => prop.Property.PropertyInfo.GetColumnName(),
                        prop => (prop.Property.PropertyInfo.IsHtmlContent() ? new ChangePropertyInfo(HtmlContentAttribute.title, HtmlContentAttribute.title) : new ChangePropertyInfo(prop.Original, prop.Current))
                    );
                }
                return new(_ee.Metadata.ClrType.GetTableName(true), extractScalarProperties(_ee.Entity, _ee.Metadata.ClrType), changes);
            }
            if (value is IFormCollection _form)
            {
                var (hasError, model, errors) = _form.TryBindFromFormAsync<ChangeEntry>().GetAwaiter().GetResult();
                if (hasError) { throw errors.ToNestedException(); }
                return model;
            }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new ChangeEntry((string)x.entityName, (Dictionary<string, object>)x.entity, (Dictionary<string, ChangePropertyInfo>)x.changeProperties)).FirstOrDefault();
        }
        private static Dictionary<string, object> extractScalarProperties(object entity, Type entityType)
        {
            if (entity == null) { return []; }
            return entityType.GetProperties().Where(prop => prop.IsMapped()).ToDictionary(prop => prop.GetColumnName(), prop => (prop.IsHtmlContent() ? HtmlContentAttribute.title : prop.GetValue(entity)));
        }
    }
}