using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    public class Collection : FieldType
    {
        private ExpressionResolver resolver = new ExpressionResolver();
        public Expressions.Expression Size { get; }
        public Collection(FieldType type, Expressions.Expression size)
        {
            this.Size = size;
            this.Type = type;
        }
        public FieldType Type { get; }

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
