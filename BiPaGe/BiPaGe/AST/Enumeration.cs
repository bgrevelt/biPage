using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using System.Linq;
using NUnit.Framework;

namespace BiPaGe.AST
{
    public class Enumeration : Element
    {
        public IList<Enumerator> Enumerators { get; }
        public String Identifier { get; }
        public FieldType Type { get; } 
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

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<Enumeration>(expected);
            var expected_enum = expected as Enumeration;

            Assert.AreEqual(expected_enum.Identifier, this.Identifier);
            this.Type.Validate(expected_enum.Type);
            Assert.AreEqual(expected_enum.Enumerators.Count, this.Enumerators.Count());
            for (int i = 0; i < this.Enumerators.Count; ++i)
                this.Enumerators[i].Validate(expected_enum.Enumerators[i]);
        }
    }
}
