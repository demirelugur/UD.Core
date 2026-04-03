namespace UD.Core.Extensions
{
    using Ganss.Xss;
    using System;
    using System.Collections.Concurrent;
    using UD.Core.Extensions.Common;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Validation;
    public static class XssExtensions
    {
        private static readonly ConcurrentDictionary<Type, StringPropAccessor[]> _cache = new();
        /// <summary>Nesne içindeki string alanları sanitize eder, boş olanları null yapar ve maksimum uzunluk kısıtlarına göre düzenler.</summary>
        public static void SanitizeAndConstrainStrings(this IHtmlSanitizer sanitizer, object entity)
        {
            Guard.ThrowIfNull(sanitizer, nameof(sanitizer));
            Guard.ThrowIfNull(entity, nameof(entity));
            string sanitized;
            var accessors = _cache.GetOrAdd(entity.GetType(), StringPropAccessor.BuildAccessors);
            foreach (var acc in accessors)
            {
                var original = acc.Getter(entity);
                if (original.IsNullOrWhiteSpace())
                {
                    if (original != null) { acc.Setter(entity, null); }
                    continue;
                }
                if (acc.SkipSanitize) { sanitized = original; }
                else
                {
                    sanitized = sanitizer.Sanitize(original).Trim();
                    if (sanitized.IsNullOrWhiteSpace())
                    {
                        acc.Setter(entity, null);
                        continue;
                    }
                }
                if (acc.MaxLength > 0) { sanitized = sanitized.SubstringUpToLength(acc.MaxLength); }
                if (String.Equals(original, sanitized, StringComparison.Ordinal)) { continue; }
                acc.Setter(entity, sanitized);
            }
        }
    }
}