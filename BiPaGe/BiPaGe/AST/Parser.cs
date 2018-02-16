using System;
using System.Collections.Generic;
using System.Linq;

namespace BiPaGe.AST
{
    public class Parser : ASTNode
    {
        public String Name { get; }
        public IList<AST.Element> Elements
        {
            get;
        }

        public Parser(SourceInfo sourceInfo, String name, IList<AST.Element> elements) : base(sourceInfo)
        {
            this.Elements = elements;
            this.Name = name;
        }

        public override void Print(int indentLevel)
        {
            var content = String.Format("Parser {0}", Name);
            PrintIndented(content, indentLevel);

            foreach(var element in Elements)
            {
                element.Print(indentLevel + 1);
            }
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            bool valid = true;
            foreach (var element in Elements)
            {
                if (!element.CheckSemantics(errors, warnings))
                    valid = false;
            }

            return valid;
        }

        public override bool Equals(IASTNode other)
        {
            var other_parser = other as Parser;
            if (other_parser.Name != this.Name)
                return false;
            if (other_parser.Elements.Count != this.Elements.Count)
                return false;

            if (this.Elements.Zip(other_parser.Elements, (l, r) => l.Equals(r)).Any(v => v == false))
                return false;

            return true;
        }
    }
}
