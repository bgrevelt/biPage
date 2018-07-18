using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    // TODO: there's some overlap here with collection. If in time we find that we always need to change the two together, we could break out a common case class
    // 'VariableSizeField' or something like that which contains all the size stuff.
    public class AsciiString : FieldType
    {
        public Expressions.Expression Size { get; }
        private ExpressionResolver resolver = new ExpressionResolver();
        public AsciiString(Expressions.Expression size) 
        {
            this.Size = size;
        }

        public override bool HasStaticSize()
        {
            return resolver.IsStaticExpression(Size);
        }

        public override uint SizeInBits()
        {
            return (uint)resolver.Resolve(Size);
        }
    }
}
