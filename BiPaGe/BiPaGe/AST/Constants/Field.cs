using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.Constants
{
    public class Field : Fixer
    {
        public Value Value { get; }

        public Field(SourceInfo sourceInfo, Value value) : base(sourceInfo)
        {
            this.Value = value;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Field fixer: {0}", Value), indentLevel);
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }
    }
}
