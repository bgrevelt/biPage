using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    public class Collection : DynamicField
    {
        public Collection(FieldType type, Expressions.Expression size)
        {
            this.Type = type;
            this.Size = size;
        }
        public FieldType Type { get; }
        public Expressions.Expression Size { get; }
    }
}
