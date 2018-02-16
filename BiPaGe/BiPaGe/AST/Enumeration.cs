using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using System.Linq;

namespace BiPaGe.AST
{
    public class Enumeration : Element
    {
        public IList<Enumerator> Enumerators { get; }
        public String Identifier { get; }
        public FieldType Type { get; } // TODO: we should include a level between the concrete type and FieldType for basic types
        public Enumeration(SourceInfo sourceInfo, String identifier, FieldType type, IList<Enumerator> enumerators) : base(sourceInfo)
        {
            this.Identifier = identifier;
            this.Enumerators = enumerators;
            this.Type = type;
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Enumeration {0}", Identifier), indentLevel);
            Type.Print(indentLevel + 1);
            foreach(var enumerator in Enumerators)
            {
                enumerator.Print(indentLevel + 1);
            }
        }

        public override bool Equals(IASTNode other)
        {
            var other_enum = other as Enumeration;

            if (other_enum.Identifier != this.Identifier)
                return false;

            if (!other_enum.Type.Equals(this.Type))
                return false;

            if (other_enum.Enumerators.Count != this.Enumerators.Count)
                return false;

            if (this.Enumerators.Zip(other_enum.Enumerators, (l, r) => l.Equals(r)).Any(v => v == false))
                return false;

            return true;
        }
    }
}
