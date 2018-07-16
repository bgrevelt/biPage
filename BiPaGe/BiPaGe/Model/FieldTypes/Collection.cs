using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    public class Collection : DynamicField
    {
        public Collection(FieldType type, Expressions.Expression size) : base(size)
        {
            this.Type = type;
        }
        public FieldType Type { get; }
    }
}
