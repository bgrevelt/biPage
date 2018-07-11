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
        public Field(String name, FieldType type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
