using System;
using UD.Core.Databases;
namespace UD.Core.Attributes
{
    /// <summary><see cref="ChangeEntry"/> üzerinde yapılan AuditLog&#39;lar üzerinden işlenen kayıtların HTML property&#39;lerini işaretlemek için geliştirilmiştir.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class HtmlContentAttribute : Attribute { }
}