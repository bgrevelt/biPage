using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.AST
{
    public abstract class Visitor
    {
        public void Visit(AST.Constants.LiteralCollection lc) { }
        public void Visit(AST.Constants.ObjectConstant oc) { }
        void Visit(AST.Constants.ObjectField of) { }

        public void Visit(AST.Expressions.Addition a) { }
        public void Visit(AST.Expressions.Subtraction s) { }
        public void Visit(AST.Expressions.Multiplication m) { }
        public void Visit(AST.Expressions.Division d) { }
        public void Visit(AST.Expressions.This t) { }

        public void Visit(AST.FieldTypes.AsciiString s) { }
        public void Visit(AST.FieldTypes.Boolean b) { }
        public void Visit(AST.FieldTypes.Float f) { }
        public void Visit(AST.FieldTypes.InlineEnumeration ie) { }
        public void Visit(AST.FieldTypes.InlineObject io) { }
        public void Visit(AST.FieldTypes.Signed s) { }
        public void Visit(AST.FieldTypes.Unsigned u) { }
        public void Visit(AST.FieldTypes.Utf8String s) { }

        public void Visit(AST.Identifiers.FieldIdentifier f) { }
        public void Visit(AST.Identifiers.Identifier i) { }

        public void Visit(AST.Literals.Boolean b) { }
        public void Visit(AST.Literals.Float f) { }
        public void Visit(AST.Literals.Integer i) { }
        public void Visit(AST.Literals.StringLiteral s) { }

        public void Visit(AST.Enumeration e) { }

        public void Visit(AST.Object o) { }
        public void Visit(AST.Field f) { }
        public void Visit(AST.FieldType t) { }


    }
}
