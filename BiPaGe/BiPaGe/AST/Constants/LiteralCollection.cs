using System;
using System.Collections.Generic;
using System.Linq;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.AST.Constants
{
    public class LiteralCollection : ASTNode, IFixer
    {
        public List<Literals.Literal> Literals { get; }

        public LiteralCollection(SourceInfo sourceInfo, List<Literals.Literal> literals) : base(sourceInfo)
        {
            this.Literals = literals;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("LiteralCollection:", indentLevel);
            foreach (var literal in Literals)
                literal.Print(indentLevel + 1);
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<LiteralCollection>(expected);
            var expected_lc = expected as LiteralCollection;
            Assert.AreEqual(expected_lc.Literals.Count, this.Literals.Count);
            for (int i = 0; i < this.Literals.Count; ++i)
                this.Literals[i].Validate(expected_lc.Literals[i]);
        }
    }
}
