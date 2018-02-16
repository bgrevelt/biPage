using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

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

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<Enumerator>(expected);
            var expected_enumerator = expected as Enumerator;
            Assert.AreEqual(expected_enumerator.Name, this.Name);
            Assert.AreEqual(expected_enumerator.Value, this.Value);
        }
    }
}
