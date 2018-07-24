using System.Diagnostics;

namespace BiPaGe.Model.FieldTypes
{
    // TODO: there's some overlap here with collection. If in time we find that we always need to change the two together, we could break out a common case class
    // 'VariableSizeField' or something like that which contains all the size stuff.
    public class AsciiString : FieldType
    {
        public Expressions.Expression Size { get; }        
        public AsciiString(Expressions.Expression size) 
        {
            this.Size = size;
        }

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
