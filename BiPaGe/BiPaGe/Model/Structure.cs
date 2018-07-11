using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public class Structure : FieldType
    {
        public String Name { get; }
        public List<Field> Fields { get; }
        public Structure(String name)
        {
            this.Fields = new List<Field>();
            this.Name = name;
        }
        public void AddField(Field field)
        {
            this.Fields.Add(field);
        }
    }
}
