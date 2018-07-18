using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public class Enumeration : DataElement
    {
    
        public FieldTypes.Integral Type { get; }
        public List<Enumerator> Enumerators { get; }
        public Enumeration(String name, FieldTypes.Integral type) : base(name)
        {
            Enumerators = new List<Enumerator>();
            this.Type = type;
        }
        public void AddEnumerator(Enumerator enumerator)
        {
            Enumerators.Add(enumerator);
        }

        public override bool HasStaticSize()
        {
            return true;
        }

        public override uint SizeInBits()
        {
            return this.Type.SizeInBits();
        }
    }
}
