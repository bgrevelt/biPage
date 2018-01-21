using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST
{
    public class Enumeration : Element
    {
        public IList<Enumerator> Enumerators { get; }
        public String Identifier { get; }

        public Enumeration(SourceInfo sourceInfo, String identifier, IList<Enumerator> enumerators) : base(sourceInfo)
        {
            this.Identifier = identifier;
            this.Enumerators = enumerators;
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Enumeration {0}", Identifier), indentLevel);
            foreach(var enumerator in Enumerators)
            {
                enumerator.Print(indentLevel + 1);
            }
        }
    }
}
