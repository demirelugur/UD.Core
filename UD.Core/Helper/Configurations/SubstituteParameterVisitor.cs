namespace UD.Core.Helper.Configurations
{
    using System.Linq.Expressions;
    internal sealed class SubstituteParameterVisitor : ExpressionVisitor
    {
        private readonly Dictionary<Expression, Expression> _sub;
        public SubstituteParameterVisitor(ParameterExpression leftParameter, ParameterExpression rightParameter)
        {
            this._sub = new Dictionary<Expression, Expression>
            {
                { rightParameter, leftParameter }
            };
        }
        protected override Expression VisitParameter(ParameterExpression node) => (this._sub.TryGetValue(node, out Expression _exp) ? _exp : node);
    }
}