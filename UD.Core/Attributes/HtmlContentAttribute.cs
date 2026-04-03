namespace UD.Core.Attributes
{
    using System;
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class HtmlContentAttribute : Attribute
    {
        public const string title = "[HTML content]";
    }
}