﻿using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.Expressions
{
    public class Multiplication : Expression
    {
        public Expression left { get; }
        public Expression right { get; }

        public Multiplication(SourceInfo sourceInfo, Expression lhs, Expression rhs) : base(sourceInfo)
        {
            left = lhs;
            right = rhs;
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(Expression other)
        {
            try
            {
                return left.Equals(((Multiplication)other).left) && right.Equals(((Multiplication)other).right);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Multiplication"), indentLevel);
            left.Print(indentLevel + 1);
            right.Print(indentLevel + 1);
        }
    }
}
