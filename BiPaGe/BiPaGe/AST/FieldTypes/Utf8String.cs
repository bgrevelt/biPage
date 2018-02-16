using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

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

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<Utf8String>(expected);
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("UTF-8 string", indentLevel);;
        }
    }
}
