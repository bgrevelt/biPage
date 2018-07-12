using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public class Enumerator 
    {
        public String Name { get; }
        public int Value { get; }

        public Enumerator(String name, int value)
        {
            this.Name = name;
            this.Value = value;
        }        
    }
}
