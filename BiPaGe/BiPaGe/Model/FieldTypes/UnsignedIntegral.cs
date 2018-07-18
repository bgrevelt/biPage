using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    public class UnsignedIntegral : Integral
    {
        public UnsignedIntegral(uint size) : base(size)
        {

        }

        public override bool HasStaticSize()
        {
            return true;
            
        }
    }
}
