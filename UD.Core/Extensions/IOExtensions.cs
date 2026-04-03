namespace UD.Core.Extensions
{
    using System.IO;
    using UD.Core.Helper;
    using UD.Core.Helper.Validation;
    public static class IOExtensions
    {
        /// <summary>Belirtilen kaynak dizinini hedef dizine kopyalar.</summary>
        /// <param name="source">Kaynak dizini temsil eden DirectoryInfo nesnesi.</param>
        /// <param name="target">Hedef dizini temsil eden DirectoryInfo nesnesi.</param>
        public static void CopyAll(this DirectoryInfo source, DirectoryInfo target)
        {
            Guard.ThrowIfNull(source, nameof(source));
            Guard.ThrowIfNull(target, nameof(target));
            Files.DirectoryCreate(target.FullName);
            foreach (var item in source.GetFiles()) { item.CopyTo(Path.Combine(target.FullName, item.Name), true); }
            foreach (var item in source.GetDirectories()) { item.CopyAll(target.CreateSubdirectory(item.Name)); }
        }
    }
}