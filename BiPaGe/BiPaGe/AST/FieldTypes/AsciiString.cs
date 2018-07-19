using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

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

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<AsciiString>(expected);
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("ASCII string", indentLevel);
        }

        public override void Accept(IFieldTypeVisitor v)
        {
            v.Visit(this);
        }
    }
}
