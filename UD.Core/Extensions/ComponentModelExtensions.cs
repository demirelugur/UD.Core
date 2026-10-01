using System.ComponentModel.DataAnnotations.Schema;
namespace UD.Core.Extensions
{
    public static class ComponentModelExtensions
    {
        /// <summary><paramref name="table"/> nesnesinin tablo adını döndürür. Eğer <paramref name="isSquareBrackets"/> true ise tablo adı köşeli parantez içinde döndürülür.</summary>
        /// <param name="table">Tablo attribute nesnesi.</param>
        /// <param name="isSquareBrackets">Tablo adının köşeli parantez içinde döndürülüp döndürülmeyeceğini belirten değer.</param>
        /// <returns>Tablo adını döndürür.</returns>
        public static string GetTableName(this TableAttribute table, bool isSquareBrackets)
        {
            ArgumentNullException.ThrowIfNull(table, nameof(table));
            var r = new List<string> { table.Schema.CoalesceOrDefault("dbo"), table.Name };
            if (isSquareBrackets) { return String.Join(".", r.Select(x => $"[{x}]").ToArray()); }
            return String.Join(".", r);
        }
    }
}