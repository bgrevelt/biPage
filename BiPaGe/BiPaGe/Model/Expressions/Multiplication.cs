namespace BiPaGe.Model.Expressions
{
    public class Multiplication : BinaryExpression
    {
      public Multiplication(Expression lhs, Expression rhs)
        {
            this.Left = lhs;
            this.Right = rhs;
        }

        public override int? Resolve()
        {
            return this.Left.Resolve() * this.Right.Resolve();
        }
    }
}
