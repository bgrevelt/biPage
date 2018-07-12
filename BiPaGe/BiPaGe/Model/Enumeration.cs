using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public class Enumeration
    {
        public String Name { get; }
        private FieldType Type { get; }
        private List<Enumerator> enumerators = new List<Enumerator>();
        public Enumeration(String name, FieldType type)
        {
            this.Name = name;
            this.Type = type;
        }
        public void AddEnumerator(Enumerator enumerator)
        {
            enumerators.Add(enumerator);
        }
        
    }
}
