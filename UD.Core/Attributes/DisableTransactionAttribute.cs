namespace UD.Core.Attributes
{
    using System;
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DisableTransactionAttribute : Attribute { }
}