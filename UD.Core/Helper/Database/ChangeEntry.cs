namespace UD.Core.Helper.Database
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Linq;
    using System.Text;
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
        private static bool AreEqual(object objA, object objB)
        {
            if (objA is byte[] _ba1 && objB is byte[] _ba2) { return _ba1.IsFileBytesEqual(_ba2); }
            return Equals(objA, objB);
        }
        private static string ToSHA512Hexadecimal(object value)
        {
            byte[] source;
            if (value == null) { source = []; }
            else if (value is String _s) { source = Encoding.UTF8.GetBytes(_s.Trim()); }
            else if (value is byte[] _byteArray) { source = _byteArray; }
            else
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new NotSupportedException($"ToSHA512Hexadecimal method only supports string, byte[] and null values"); }
                throw new NotSupportedException($"ToSHA512Hexadecimal metodu sadece string, byte[] ve null değerlerini destekler.");
            }
            return source.ToSHA512Hexadecimal();
        }
        /// <summary><paramref name="value"/> için tanımlanan nesneler: ChangeEntry, EntityEntry, AnonymousObjectClass</summary>
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
                    .Where(prop => !prop.PropertyInfo.IsSkipAuditLog() && prop.PropertyInfo.IsMapped())
                    .Select(prop => new
                    {
                        prop.PropertyInfo,
                        Original = _ee.OriginalValues[prop],
                        Current = _ee.CurrentValues[prop]
                    })
                    .Where(x => !AreEqual(x.Original, x.Current))
                    .ToDictionary(
                        prop => prop.PropertyInfo.GetColumnName(),
                        prop => ((prop.PropertyInfo.PropertyType == typeof(byte[]) || prop.PropertyInfo.IsHtmlContent()) ? new ChangePropertyInfo(ToSHA512Hexadecimal(prop.Original), ToSHA512Hexadecimal(prop.Current)) : new ChangePropertyInfo(prop.Original, prop.Current))
                    );
                }
                return new(_ee.Metadata.ClrType.GetTableName(true), extractScalarProperties(_ee.Entity, _ee.Metadata.ClrType), changes);
            }
            /*
            if (value is IFormCollection _form)
            {
                var (hasError, model, errors) = _form.TryBindFromFormAsync<ChangeEntry>().GetAwaiter().GetResult();
                if (hasError) { throw errors.ToNestedException(); }
                return model;
            }
            */
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new ChangeEntry((string)x.entityName, (Dictionary<string, object>)x.entity, (Dictionary<string, ChangePropertyInfo>)x.changeProperties)).FirstOrDefault();
        }
        private static Dictionary<string, object> extractScalarProperties(object entity, Type entityType)
        {
            if (entity == null) { return []; }
            return entityType.GetProperties().Where(prop => !prop.IsSkipAuditLog() && prop.IsMapped()).ToDictionary(prop => prop.GetColumnName(), prop => ((prop.PropertyType == typeof(byte[]) || prop.IsHtmlContent()) ? ToSHA512Hexadecimal(prop.GetValue(entity)) : prop.GetValue(entity)));
        }
    }
}