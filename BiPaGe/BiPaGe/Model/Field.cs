using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public class Field
    {
        public Field(String name, uint offset, uint size)
        {
            this.name = name;
            this.offset = offset;
            this.size = size;
        }
        public uint offset; // The offset of this field from the start of the parent object in bits
        public uint size; // the size of this field in bits
        public String name;
        //public Type type; // TODO

        /*
         * I think what we have to move towards is that a field has a size and an offset
         * The size can be either static (e.g. a uint) or dynamic (e.g an expression: field_id, add, mul, div, ...)
         * The offset is a collection of size (either static or dynamic)
         * */
    }
}
