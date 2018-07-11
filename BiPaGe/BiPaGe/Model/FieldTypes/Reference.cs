using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.FieldTypes
{
    class Reference : FieldType
    {
        public String Name { get; } 
        public Reference(String name)
        {
            this.Name = name;
        }
    }
}
