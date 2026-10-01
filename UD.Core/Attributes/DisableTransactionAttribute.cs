using UD.Core.Middlewares;
namespace UD.Core.Attributes
{
    /// <summary><see cref="TransactionMiddleware{TContext}"/> için oluşturulmuş bir özniteliktir. Bu öznitelik, uygulandığı sınıf veya yöntemdeki işlemlerin bir veritabanı işlemi (transaction) içinde yürütülmesini devre dışı bırakır. Bu, belirli durumlarda işlem yönetimini manuel olarak yapmak isteyen geliştiriciler için faydalı olabilir. Örneğin, bazı işlemler zaten başka bir işlem içinde yürütülüyor olabilir veya işlem yönetimi gerektirmeyen durumlar olabilir. Bu özniteliği kullanarak, bu tür durumlarda otomatik işlem yönetimini devre dışı bırakabilirsiniz.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DisableTransactionAttribute : Attribute { }
}