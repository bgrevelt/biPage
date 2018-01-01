using System;
using System.Collections.Generic;

namespace BiPaGe.AST
{
    public class Parser : IASTNode
    {
        public String Name { get; }
        public IList<AST.Object> Objects
        {
            get;
        }

        public Parser(SourceInfo sourceInfo, String name, IList<AST.Object> objects) : base(sourceInfo)
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

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
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
