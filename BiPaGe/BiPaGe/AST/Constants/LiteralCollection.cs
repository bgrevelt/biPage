using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

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
    }
}
