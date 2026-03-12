namespace UD.Core.Helper.Database
{
    using static UD.Core.Helper.GlobalConstants;
    public sealed class SqlConverter
    {
        /// <summary> Guid değerini SQL Server UNIQUEIDENTIFIER tipine dönüştüren CONVERT ifadesini oluşturur.</summary>
        public static string ToUniqueIdentifier(Guid guid) => $"CONVERT(UNIQUEIDENTIFIER, '{guid.ToString().ToUpper()}')";
        /// <summary> DateOnly değerini SQL Server DATE tipine dönüştüren CONVERT ifadesini oluşturur. </summary>
        public static string ToDate(DateOnly date) => $"CONVERT(DATE, '{date.ToString(DateConstants.yyyyMMdd)}')";
        /// <summary> DateTime değerini SQL Server DATETIME tipine dönüştüren CONVERT ifadesini oluşturur. </summary>
        public static string ToDateTime(DateTime dateTime) => $"CONVERT(DATETIME, '{dateTime:yyyy-MM-dd HH:mm:ss:fff}')";
    }
}