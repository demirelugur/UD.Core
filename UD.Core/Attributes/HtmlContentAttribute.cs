namespace UD.Core.Attributes
{
    using System;
    using UD.Core.Helper.Database;
    /// <summary>
    /// <see cref="ChangeEntry"/> üzerinde yapılan AuditLog&#39;lar üzerinden işlenen kayıtların HTML property&#39;lerini işaretlemek için geliştirilmiştir.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class HtmlContentAttribute : Attribute { }
}