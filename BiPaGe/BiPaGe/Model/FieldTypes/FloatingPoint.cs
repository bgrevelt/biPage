using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    public class FloatingPoint : FieldType
    {
        public uint Size { get; } 
        public FloatingPoint(uint size)
        {
            this.Size = size;
        }
    }
}
