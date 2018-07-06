using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public class Enumeration
    {
        private Scope scope; // The scope in which the enumeration was defined. Empty string indicates global scope. A namei indicates that the enumeration is scoped in a type
        private String name;
        private List<Enumerator> enumerators = new List<Enumerator>();
        public Enumeration(String name, Scope scope)
        {
            this.scope = scope.Clone();
            this.name = name;
        }
        public void AddEnumerator(Enumerator enumerator)
        {
            enumerators.Add(enumerator);
        }
        
    }
}
