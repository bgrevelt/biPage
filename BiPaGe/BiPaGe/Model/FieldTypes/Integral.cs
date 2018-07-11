using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    public class Integral : FieldType
    {
        public Integral(bool signed, uint size)
        {
            this.signed = signed;
            this.size = size;
        }
        public bool signed; // is this a signed type
        public uint size;   // size in bits
    }
}
