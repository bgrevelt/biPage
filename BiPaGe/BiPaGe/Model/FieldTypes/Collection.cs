using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    public class Collection : FieldType
    {
        public Collection(FieldType type, Expressions.Expression size)
        {
            this.type = type;
            this.size = size;
        }
        public FieldType type { get; }
        public Expressions.Expression size { get; }
    }
}
