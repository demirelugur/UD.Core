namespace UD.Core.Extensions
{
    using System.Linq.Expressions;
    using System.Reflection;
    using UD.Core.Extensions.Common;
    using UD.Core.Helper;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Validation;
    public static class ExpressionExtensions
    {
        /// <summary>Verilen ifadenin adını alır.</summary>
        /// <param name="expression">İfade.</param>
        /// <returns>İfadenin adı.</returns>
        /// <exception cref="ArgumentException">İfade geçersiz ise fırlatılır.</exception>
        public static string GetMemberName(this Expression expression)
        {
            Guard.ThrowIfNull(expression, nameof(expression));
            if (expression is LambdaExpression _lambda) { expression = _lambda.Body; }
            var result = "";
            if (expression is MemberExpression _me) { result = _me.Member.Name; }
            else if (expression is UnaryExpression _ue)
            {
                if (_ue.Operand is MemberExpression _ume) { result = _ume.Member.Name; }
                else if (_ue.Operand is MethodCallExpression _umce && _umce.Object is ConstantExpression _uce && _uce.Value != null)
                {
                    if (_uce.Value is MemberInfo _mi) { result = _mi.Name; }
                    else if (TryValidators.TryGetProperty(_uce.Value, nameof(MemberInfo.Name), out string _name)) { result = _name; }
                }
            }
            if (result.IsNullOrEmpty())
            {
                if (ValidationChecks.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The value of \"{expression}\" is incompatible!", nameof(expression)); }
                throw new ArgumentException($"\"{expression}\" değeri uyumsuzdur!", nameof(expression));
            }
            return result;
        }
        /// <summary>Expression&lt;Func&lt;T, bool&gt;&gt; türündeki iki predicate&#39;i mantıksal AND (&amp;&amp;) operatörü ile birleştirir.</summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) => left.CombineLambdas(right, ExpressionType.AndAlso);
        /// <summary>Belirtilen koşul <see langword="true"/> ise ikinci predicate&#39;i AND ile birleştirir, değilse mevcut ifadeyi olduğu gibi döndürür. Dinamik filtrelerde koşullu ekleme için kullanılır.</summary>
        public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> left, bool condition, Expression<Func<T, bool>> right) => (condition ? left.And(right) : left);
        /// <summary>Expression&lt;Func&lt;T, bool&gt;&gt; türündeki iki predicate&#39;i mantıksal OR (||) operatörü ile birleştirir</summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) => left.CombineLambdas(right, ExpressionType.OrElse);
        /// <summary>Belirtilen koşul <see langword="true"/> ise ikinci predicate&#39;i OR ile birleştirir, değilse mevcut ifadeyi olduğu gibi döndürür. Dinamik filtrelerde koşullu ekleme için kullanılır.</summary>
        public static Expression<Func<T, bool>> OrIf<T>(this Expression<Func<T, bool>> left, bool condition, Expression<Func<T, bool>> right) => (condition ? left.Or(right) : left);
        private static Expression<Func<T, bool>> CombineLambdas<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, ExpressionType expressionType)
        {
            var leftParameter = left.Parameters[0];
            var visitor = new SubstituteParameterVisitor(leftParameter, right.Parameters[0]);
            var body = Expression.MakeBinary(expressionType, left.Body, visitor.Visit(right.Body));
            return Expression.Lambda<Func<T, bool>>(body, leftParameter);
        }
    }
}