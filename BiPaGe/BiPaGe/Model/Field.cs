using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public class Field 
    {
        public String Name { get; }
        public FieldType Type { get; }
        public String OffsetFrom { get; }
        public uint Offset { get; }
        public Field(String name, FieldType type, uint offset, String offsetFrom)
        {
            this.Name = name;
            this.Type = type;
            this.Offset = offset;
            this.OffsetFrom = offsetFrom;
        }
        public bool HasStaticSize()
        {
            return Type.HasStaticSize();
        }

        public uint SizeInBits()
        {
            return Type.SizeInBits();
        }
    }
}
