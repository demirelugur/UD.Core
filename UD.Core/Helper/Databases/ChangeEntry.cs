namespace UD.Core.Helper.Databases
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Linq;
    using System.Reflection;
    using UD.Core.Extensions;
    public sealed class ChangeEntry
    {
        public string entityName { get; set; }
        public object entityPKValue { get; set; }
        public Dictionary<string, object> entity { get; set; }
        public Dictionary<string, ChangePropertyInfo> changeProperties { get; set; }
        public ChangeEntry() : this(default, default, default, default) { }
        public ChangeEntry(string entityName, object entityPKValue, Dictionary<string, object> entity, Dictionary<string, ChangePropertyInfo> changeProperties)
        {
            this.entityName = entityName ?? "";
            this.entityPKValue = entityPKValue;
            this.entity = entity ?? [];
            this.changeProperties = changeProperties ?? [];
        }
        private static bool IsMapped(PropertyInfo propertyInfo) => (!propertyInfo.IsSkipAuditLog() && propertyInfo.IsMapped());
        private static bool IsTypeByteArrayOrHtmlContent(PropertyInfo propertyInfo) => (propertyInfo.PropertyType == typeof(byte[]) || propertyInfo.IsHtmlContent());
        private static bool AreEqual(object objA, object objB)
        {
            if (objA is byte[] _ba1 && objB is byte[] _ba2) { return _ba1.IsAbsoluteEqual(_ba2); }
            return Equals(objA, objB);
        }
        private static string ComputeHash(object value)
        {
            if (value == null) { return Array.Empty<byte>().ComputeHash(true); }
            if (value is String _s) { return _s.ComputeHash(true); }
            if (value is byte[] _byteArray) { return _byteArray.ComputeHash(true); }
            if (Checks.IsEnglishCurrentUICulture) { throw new NotSupportedException($"{nameof(ComputeHash)} method only supports null, string and byte[] values"); }
            throw new NotSupportedException($"{nameof(ComputeHash)} metodu sadece null, string ve byte[] değerlerini destekler.");
        }
        private static object GetPKValue(object entity, Type entityType)
        {
            var pks = entityType.GetProperties().Where(x => x.IsPK()).Select(x => x.Name).ToArray();
            if (pks.Length == 1) { return entityType.GetProperty(pks[0]).GetValue(entity); }
            return null;
        }
        private static Dictionary<string, object> ExtractScalarProperties(object entity, Type entityType) => entityType.GetProperties().Where(IsMapped).ToDictionary(x => x.GetColumnName(), x => (IsTypeByteArrayOrHtmlContent(x) ? ComputeHash(x.GetValue(entity)) : x.GetValue(entity)));
        /// <summary><paramref name="value"/> için tanımlanan nesneler: ChangeEntry, EntityEntry, AnonymousObjectClass</summary>
        public static ChangeEntry ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is ChangeEntry _ce) { return _ce; }
            if (value is EntityEntry _ee)
            {
                if (_ee.Entity == null) { return new(); }
                Dictionary<string, ChangePropertyInfo> changes = null;
                if (_ee.State == EntityState.Modified)
                {
                    changes = _ee.OriginalValues.Properties
                    .Where(x => IsMapped(x.PropertyInfo))
                    .Select(x => new
                    {
                        x.PropertyInfo,
                        Original = _ee.OriginalValues[x],
                        Current = _ee.CurrentValues[x]
                    })
                    .Where(x => !AreEqual(x.Original, x.Current))
                    .ToDictionary(
                        x => x.PropertyInfo.GetColumnName(),
                        x => (IsTypeByteArrayOrHtmlContent(x.PropertyInfo) ? new ChangePropertyInfo(ComputeHash(x.Original), ComputeHash(x.Current)) : new ChangePropertyInfo(x.Original, x.Current))
                    );
                }
                return new(_ee.Metadata.ClrType.GetTableName(true), GetPKValue(_ee.Entity, _ee.Metadata.ClrType), ExtractScalarProperties(_ee.Entity, _ee.Metadata.ClrType), changes);
            }
            /*
            if (value is IFormCollection _form)
            {
                var (hasError, model, errors) = _form.TryBindFromFormAsync<ChangeEntry>().GetAwaiter().GetResult();
                if (hasError) { throw errors.ToNestedException(); }
                return model;
            }
            */
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new ChangeEntry((string)x.entityName, (object)x.entityPKValue, (Dictionary<string, object>)x.entity, (Dictionary<string, ChangePropertyInfo>)x.changeProperties)).FirstOrDefault();
        }
    }
}