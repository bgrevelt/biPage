namespace BiPaGe.Model.Expressions
{
    public class Subtraction : BinaryExpression
    {
        public Subtraction(Expression lhs, Expression rhs)
        {
            this.Left = lhs;
            this.Right = rhs;
        }

        public override int? Resolve()
        {
            return this.Left.Resolve() - this.Right.Resolve();
        }
    }
}
