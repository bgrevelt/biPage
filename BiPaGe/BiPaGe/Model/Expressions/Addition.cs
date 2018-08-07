namespace BiPaGe.Model.Expressions
{
    public class Addition : BinaryExpression
    {
        public Addition(Expression lhs, Expression rhs) 
        {
            this.Left = lhs;
            this.Right = rhs;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override int? Resolve()
        {
            return this.Left.Resolve() + this.Right.Resolve();
        }
    }
}
