using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.AST
{
    public class Enumerator : ASTNode
    {
        public String Name { get; }
        public int Value
        {
            get { return int.Parse(this.Original_value); }
        }
        private String Original_value;
        public Enumerator(SourceInfo sourceinfo, String name, String value) : base(sourceinfo)
        {
            this.Name = name;
            this.Original_value = value;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Enumerator {0} = {1}", Name, Original_value), indentLevel);
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            // TODO: check if the we can convert value to an integer
            throw new NotImplementedException();
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<Enumerator>(expected);
            var expected_enumerator = expected as Enumerator;
            Assert.AreEqual(expected_enumerator.Name, this.Name);
            Assert.AreEqual(expected_enumerator.Original_value, this.Original_value);
        }
    }
}
