using System.Diagnostics;

namespace BiPaGe.Model.FieldTypes
{
    public class Collection : FieldType
    {
        public Expressions.Expression Size { get; }
        public Collection(FieldType type, Expressions.Expression size)
        {
            this.Size = size;
            this.Type = type;
        }
        public FieldType Type { get; }

        public override bool HasStaticSize()
        {
            return Size.Resolve() != null;
        }

        public override uint SizeInBits()
        {
            var value = Size.Resolve();
            Debug.Assert(value > 0);
            return (uint)value;
        }

        public override void Accept(IFieldTypeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
