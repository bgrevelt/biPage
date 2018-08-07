using System;
namespace BiPaGe.Model.Expressions
{
    public class FieldIdentifier : Expression
    {
        public String Identifier { get; }
        public FieldIdentifier(String identifier)
        {
            this.Identifier = identifier;
        }

        public override int? Resolve()
        {
            return null;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
