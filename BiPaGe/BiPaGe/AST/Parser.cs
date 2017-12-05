using System;
using System.Collections.Generic;

namespace BiPaGe.AST
{
    public class Parser : IASTNode
    {
        public String Name { get; }
        public IEnumerable<AST.Object> Objects
        {
            get;
        }

        public Parser(String name, IEnumerable<AST.Object> objects)
        {
            this.Objects = objects;
            this.Name = name;
        }

        public override void Print(int indentLevel)
        {
            var content = String.Format("Parser {0}", Name);
            PrintIndented(content, indentLevel);

            foreach(var o in Objects)
            {
                o.Print(indentLevel + 1);
            }
        }

        public override bool CheckSemantics(IList<string> errors, IList<string> warnings)
        {
            bool valid = true;
            foreach (var o in Objects)
            {
                if (!o.CheckSemantics(errors, warnings))
                    valid = false;
            }

            return valid;
        }
    }
}
