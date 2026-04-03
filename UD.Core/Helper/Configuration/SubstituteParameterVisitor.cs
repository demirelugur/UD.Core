namespace UD.Core.Helper.Configuration
{
    using System.Linq.Expressions;
    internal class SubstituteParameterVisitor : ExpressionVisitor
    {
        private readonly Dictionary<Expression, Expression> sub;
        public SubstituteParameterVisitor(ParameterExpression leftParameter, ParameterExpression rightParameter)
        {
            this.sub = new Dictionary<Expression, Expression>
            {
                { rightParameter, leftParameter }
            };
        }
        protected override Expression VisitParameter(ParameterExpression node) => (this.sub.TryGetValue(node, out Expression _exp) ? _exp : node);
    }
}