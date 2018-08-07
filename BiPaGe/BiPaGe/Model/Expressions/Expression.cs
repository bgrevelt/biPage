namespace BiPaGe.Model.Expressions
{
    public abstract class Expression
    {
        public abstract int? Resolve();
        public abstract void Accept(IExpressionVisitor visitor);
    }
}
