using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    public abstract class DynamicField : FieldType
    {
        public Expressions.Expression Size { get; }
        public DynamicField(Expressions.Expression size)
        {
            this.Size = size;
        }
    }
}
