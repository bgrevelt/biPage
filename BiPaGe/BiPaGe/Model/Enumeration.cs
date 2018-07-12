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
        public FieldType Type { get; }
        public List<Enumerator> Enumerators { get; }
        public Enumeration(String name, FieldType type)
        {
            Enumerators = new List<Enumerator>();
            this.Name = name;
            this.Type = type;
        }
        public void AddEnumerator(Enumerator enumerator)
        {
            Enumerators.Add(enumerator);
        }
        
    }
}
