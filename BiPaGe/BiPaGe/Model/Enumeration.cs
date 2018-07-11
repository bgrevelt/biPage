using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public class Enumeration
    {
        public String name { get; }
        private FieldType type { get; }
        private List<Enumerator> enumerators = new List<Enumerator>();
        public Enumeration(String name, FieldType type)
        {
            this.name = name;
            this.type = type;
        }
        public void AddEnumerator(Enumerator enumerator)
        {
            enumerators.Add(enumerator);
        }
        
    }
}
