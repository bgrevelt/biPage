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

        public override bool Equals(IASTNode other)
        {
            try
            {
                var other_lc = other as LiteralCollection;
                if (other_lc.Literals.Count != this.Literals.Count)
                    return false;

                for (int i = 0; i < this.Literals.Count; ++i)
                {
                    var this_literal = this.Literals[i];
                    var other_literal = other_lc.Literals[i];
                    if (!this_literal.Equals(other_literal))
                        return false;
                }

                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}
