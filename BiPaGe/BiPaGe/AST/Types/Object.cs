using System;
using System.Collections.Generic;

namespace BiPaGe.AST.Types
{
    public class Object : IASTNode
    {
        public String identifier
        {
            get;
        }

        public IEnumerable<Field> fields
        {
            get;
        }

        public Object(String identifier, IEnumerable<Field> fields)
        {
            this.identifier = identifier;
            this.fields = fields;
        }
    }
}
