namespace BiPaGe.Model.Expressions
{
    public abstract class BinaryExpression : Expression
    {
        public Expression Left { get; protected set; }
        public Expression Right { get; protected set; }

    }
}
