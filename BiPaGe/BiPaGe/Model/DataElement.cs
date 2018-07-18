using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public abstract class DataElement : FieldType
    {
        public String Name { get; }
        public DataElement(String name)
        {
            this.Name = name;
        }
    }
}
