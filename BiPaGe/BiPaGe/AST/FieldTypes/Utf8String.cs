using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.FieldTypes
{
    public class Utf8String : AST.FieldType
    {
        public Utf8String(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IASTNode other)
        {
            return other.GetType() == typeof(Utf8String);
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("UTF-8 string", indentLevel);;
        }
    }
}
