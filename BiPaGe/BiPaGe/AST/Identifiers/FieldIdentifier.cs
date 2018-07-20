using System;
using System.Collections.Generic;
using BiPaGe.AST.Expressions;
using NUnit.Framework;

namespace BiPaGe.AST.Identifiers
{
    public class FieldIdentifier : ASTNode, IExpression
    {
        public  String Id { get; }
        public FieldIdentifier(SourceInfo sourceIfo, String id) : base(sourceIfo)
        {
            this.Id = id;
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            // TODO: we should pass in a list of known objects and their fields
            return true;
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<FieldIdentifier>(expected);
            Assert.AreEqual(((FieldIdentifier)expected).Id, this.Id);
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Field id: {0}", Id), indentLevel);
        }

        public void Accept(IExpressionVisitor v)
        {
            v.Visit(this);
        }
    }
}
