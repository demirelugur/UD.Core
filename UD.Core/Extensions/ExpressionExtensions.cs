namespace UD.Core.Extensions
{
    using System.Linq.Expressions;
    public static class LinqExtension
    {
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
            var visitor = new SubstituteParameterVisitor(new Dictionary<Expression, Expression>
            {
                { right.Parameters[0], leftParameter }
            });
            var body = Expression.MakeBinary(expressionType, left.Body, visitor.Visit(right.Body));
            return Expression.Lambda<Func<T, bool>>(body, leftParameter);
        }
    }
    internal class SubstituteParameterVisitor : ExpressionVisitor
    {
        public Dictionary<Expression, Expression> sub { get; set; }
        public SubstituteParameterVisitor() : this(default) { }
        public SubstituteParameterVisitor(Dictionary<Expression, Expression> sub)
        {
            this.sub = sub ?? [];
        }
        protected override Expression VisitParameter(ParameterExpression node) => (this.sub.TryGetValue(node, out Expression _exp) ? _exp : node);
    }
}