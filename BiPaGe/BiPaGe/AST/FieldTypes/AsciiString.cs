using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.FieldTypes
{
    public class AsciiString : AST.FieldType
    {
        public AsciiString(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IASTNode other)
        {
            return other.GetType() == typeof(AsciiString);
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("ASCII string", indentLevel);
        }
    }
}
