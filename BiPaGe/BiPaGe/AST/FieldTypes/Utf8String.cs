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

        public override uint SizeInBits()
        {
            /* TODO:
             * Uhm, maybe it wasn't such a good idea to include this type. I don't think we ever need it and what does
             * utf8_string[12] mean? 12 UTF-8 characters (e.g somewhere between 12 and 48 bytes), or 12 bytes containing anywhere between 3 and 12 symbols?
             * */
            throw new NotImplementedException();
        }
    }
}
