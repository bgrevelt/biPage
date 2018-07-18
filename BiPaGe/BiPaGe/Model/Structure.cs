using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public class Structure : DataElement
    {
        public List<Field> Fields { get; }
        public Structure(String name) : base(name)
        {
            this.Fields = new List<Field>();
        }
        public void AddField(Field field)
        {
            this.Fields.Add(field);
        }

        public override bool HasStaticSize()
        {
            foreach (var field in Fields)
                if (!field.HasStaticSize())
                    return false;

            return true;
        }

        public override uint SizeInBits()
        {
            uint sum = 0;
            foreach (var field in Fields)
                sum += field.SizeInBits();

            return sum;
        }
    }
}
