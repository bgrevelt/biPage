namespace BiPaGe.Model.Expressions
{
    public class Number : Expression
    {
        public int Value { get; }
        public Number(int value)
        {
            this.Value = value;
        }

        public override int? Resolve()
        {
            return this.Value;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
