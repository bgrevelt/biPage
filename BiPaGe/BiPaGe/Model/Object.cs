using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public class Object
    {
        public Object(String name)
        {
            this.name = name;
            this.fields = new List<Field>();
        }
        public void AddField(Field field)
        {
            fields.Add(field);
        }
        public String name { get; }
        public List<Field> fields { get; }
    }
}
