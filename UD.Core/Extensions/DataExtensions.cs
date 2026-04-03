namespace UD.Core.Extensions
{
    using System.Data;
    using UD.Core.Extensions;
    using UD.Core.Extensions.Common;
    using UD.Core.Helper;
    using UD.Core.Helper.Validation;
    public static class DataExtensions
    {
        /// <summary>Verilen <see cref="SqlDbType"/> enum değerini, SQL Server sistem tür kimliğine (<c>[system_type_id]</c>) dönüştürür. Bu kimlikler, SQL Server&#39;ın [sys].[types] sistem tablosunda bulunan ve her veri türü için benzersiz olan sayısal değerlerdir.</summary>
        /// <param name="type">Dönüştürülecek <see cref="SqlDbType"/> enum değeri.</param>
        /// <returns>SQL Server sistem tür kimliği (<c>[system_type_id]</c>) değeri</returns>
        /// <exception cref="NotSupportedException">Desteklenmeyen bir <see cref="SqlDbType"/> değeri verildiğinde fırlatılır.</exception>
        /// <remarks>SELECT [name], [system_type_id] FROM [sys].[types]</remarks>
        public static int ToSystemTypeId(this SqlDbType type)
        {
            return type switch
            {
                SqlDbType.Image => 34,
                SqlDbType.Text => 35,
                SqlDbType.UniqueIdentifier => 36,
                SqlDbType.Date => 40,
                SqlDbType.Time => 41,
                SqlDbType.DateTime2 => 42,
                SqlDbType.DateTimeOffset => 43,
                SqlDbType.TinyInt => 48,
                SqlDbType.SmallInt => 52,
                SqlDbType.Int => 56,
                SqlDbType.SmallDateTime => 58,
                SqlDbType.Real => 59,
                SqlDbType.Money => 60,
                SqlDbType.DateTime => 61,
                SqlDbType.Float => 62,
                SqlDbType.NText => 99,
                SqlDbType.Bit => 104,
                SqlDbType.Decimal => 106,
                SqlDbType.SmallMoney => 122,
                SqlDbType.BigInt => 127,
                SqlDbType.VarBinary => 165,
                SqlDbType.VarChar => 167,
                SqlDbType.Binary => 173,
                SqlDbType.Char => 175,
                SqlDbType.Timestamp => 189,
                SqlDbType.NVarChar => 231,
                SqlDbType.NChar => 239,
                SqlDbType.Xml => 241,
                _ => throw Utilities.ThrowNotSupportedForEnum<SqlDbType>()
            };
        }
        /// <summary>Verilen <see cref="SqlDbType"/> enum değerini, ADO.NET&#39;in genel veri türü olan <see cref="DbType"/>&#39;a dönüştürür. SQL Server&#39;a özgü veri türlerini platform bağımsız <see cref="DbType"/> türlerine çevirir.</summary>
        /// <param name="type">Dönüştürülecek <see cref="SqlDbType"/> enum değeri.</param>
        /// <returns>Eşdeğer <see cref="DbType"/> enum değeri.</returns>
        /// <exception cref="NotSupportedException">Desteklenmeyen bir <see cref="SqlDbType"/> değeri verildiğinde fırlatılır.</exception>
        public static DbType ToDbType(this SqlDbType type)
        {
            return type switch
            {
                SqlDbType.UniqueIdentifier => DbType.Guid,
                SqlDbType.Date => DbType.Date,
                SqlDbType.Time => DbType.Time,
                SqlDbType.DateTime2 => DbType.DateTime2,
                SqlDbType.DateTimeOffset => DbType.DateTimeOffset,
                SqlDbType.TinyInt => DbType.Byte,
                SqlDbType.SmallInt => DbType.Int16,
                SqlDbType.Int => DbType.Int32,
                SqlDbType.Real => DbType.Single,
                SqlDbType.Money => DbType.Currency,
                SqlDbType.DateTime => DbType.DateTime,
                SqlDbType.Float => DbType.Double,
                SqlDbType.Bit => DbType.Boolean,
                SqlDbType.Decimal => DbType.Decimal,
                SqlDbType.BigInt => DbType.Int64,
                SqlDbType.VarChar => DbType.AnsiString,
                SqlDbType.Binary => DbType.Binary,
                SqlDbType.VarBinary => DbType.Binary,
                SqlDbType.Char => DbType.AnsiStringFixedLength,
                SqlDbType.NVarChar => DbType.String,
                SqlDbType.NChar => DbType.StringFixedLength,
                SqlDbType.Xml => DbType.Xml,
                _ => throw Utilities.ThrowNotSupportedForEnum<SqlDbType>()
            };
        }
        /// <summary>Verilen <see cref="DbType"/> enum değerini, SQL Server&#39;a özgü <see cref="SqlDbType"/> enum değerine dönüştürür. ADO.NET&#39;in genel veri türleri (<see cref="DbType"/>) ile SQL Server&#39;ın özel veri türleri (<see cref="SqlDbType"/>) arasında eşleme yapar.</summary>
        /// <param name="type">Dönüştürülecek <see cref="DbType"/> enum değeri.</param>
        /// <returns>Eşdeğer <see cref="SqlDbType"/> enum değeri.</returns>
        /// <exception cref="NotSupportedException">Desteklenmeyen bir <see cref="DbType"/> değeri verildiğinde fırlatılır.</exception>
        public static SqlDbType ToSqlDbType(this DbType type)
        {
            return type switch
            {
                DbType.Guid => SqlDbType.UniqueIdentifier,
                DbType.Date => SqlDbType.Date,
                DbType.Time => SqlDbType.Time,
                DbType.DateTime2 => SqlDbType.DateTime2,
                DbType.DateTimeOffset => SqlDbType.DateTimeOffset,
                DbType.Byte => SqlDbType.TinyInt,
                DbType.Int16 => SqlDbType.SmallInt,
                DbType.Int32 => SqlDbType.Int,
                DbType.Single => SqlDbType.Real,
                DbType.Currency => SqlDbType.Money,
                DbType.DateTime => SqlDbType.DateTime,
                DbType.Double => SqlDbType.Float,
                DbType.Boolean => SqlDbType.Bit,
                DbType.Decimal => SqlDbType.Decimal,
                DbType.Int64 => SqlDbType.BigInt,
                DbType.AnsiString => SqlDbType.VarChar,
                DbType.Binary => SqlDbType.VarBinary,
                DbType.AnsiStringFixedLength => SqlDbType.Char,
                DbType.String => SqlDbType.NVarChar,
                DbType.StringFixedLength => SqlDbType.NChar,
                DbType.Xml => SqlDbType.Xml,
                _ => throw Utilities.ThrowNotSupportedForEnum<DbType>()
            };
        }
        /// <summary>Belirtilen sütun adını kullanarak <see cref="IDataReader"/> içinden değeri okur ve verilen türde (<typeparamref name="TKey"/>) döndürür. Eğer sütun değeri <c>null</c>, <see cref="DBNull"/> ya da dönüştürülemeyen bir değer içeriyorsa, türün varsayılan değerini (<c>default</c>) geri döner.</summary>
        /// <typeparam name="TKey">Dönüştürülmek istenen hedef tür.</typeparam>
        /// <param name="reader">Veri kaynağından okuma yapan <see cref="IDataReader"/> nesnesi.</param>
        /// <param name="key">Okunacak sütunun adı.</param>
        /// <returns>Sütun değeri başarıyla dönüştürülebilirse <typeparamref name="TKey"/> tipinde değer, aksi durumda varsayılan değer döner.</returns>
        public static TKey ParseOrDefault<TKey>(this IDataReader reader, string key)
        {
            Guard.ThrowIfNull(reader, nameof(reader));
            try
            {
                key = key.ToStringOrEmpty();
                var value = reader[key];
                if (value == null || value == DBNull.Value) { return default; }
                return value.ToString().ParseOrDefault<TKey>();
            }
            catch { return default; }
        }
    }
}