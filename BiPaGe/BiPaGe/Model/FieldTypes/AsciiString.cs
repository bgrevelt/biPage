using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    // TODO: not sure about this one yet. Needs some more thought...
    public class AsciiString : Collection
    {
        public AsciiString(Expressions.Expression size) : base( new Integral(false, 8), size)
        { 
        }
    }
}
