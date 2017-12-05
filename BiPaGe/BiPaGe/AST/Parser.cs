using System;
using System.Collections.Generic;

namespace BiPaGe.AST
{
    public class Parser : IASTNode
    {
        public String Name { get; }
        public IEnumerable<Object> Objects
        {
            get;
        }

        public Parser(String name, IEnumerable<Object> objects)
        {
            this.Objects = objects;
            this.Name = name;
        }


    }
}
