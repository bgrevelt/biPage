using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST
{
    public class Enumerator : ASTNode
    {
        public String Name { get; }
        public String Value { get; }
        public Enumerator(SourceInfo sourceinfo, String name, String value) : base(sourceinfo)
        {
            this.Name = name;
            this.Value = value;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Enumerator {0} = {1}", Name, Value), indentLevel);
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IASTNode other)
        {
            throw new NotImplementedException();
        }
    }
}
