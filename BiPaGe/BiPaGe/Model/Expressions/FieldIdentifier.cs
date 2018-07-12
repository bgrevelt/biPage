using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.Expressions
{
    public class FieldIdentifier : Expression
    {
        public String Identifier { get; }
        public FieldIdentifier(String identifier)
        {
            this.Identifier = identifier;
        }
    }
}
