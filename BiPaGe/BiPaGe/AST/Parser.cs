using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

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

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<Parser>(expected);
            var expected_parser = expected as Parser;
            Assert.AreEqual(expected_parser.Name, this.Name);
            Assert.AreEqual(expected_parser.Elements.Count , this.Elements.Count);
            for (int i = 0; i < this.Elements.Count; ++i)
                this.Elements[i].Validate(expected_parser.Elements[i]);
        }
    }
}
