using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    public abstract class Integral : StaticField
    {
        public Integral(uint size)
        {
            this.size = size;
        }
        public uint size;   // size in bits
    }
}
