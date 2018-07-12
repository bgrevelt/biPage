using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BiPaGe.AST
{
    public class Object : Element
    {
        public String Identifier
        {
            get;
        }

        public IList<Field> Fields
        {
            get;
        }

        public Object(SourceInfo sourceInfo, String identifier, IList<Field> fields) : base(sourceInfo)
        {
            this.Identifier = identifier;
            this.Fields = fields;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Object {0}: ", Identifier), indentLevel);

            foreach (var field in Fields)
            {
                field.Print(indentLevel + 1);
            }
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            bool semantics_valid = true;

            foreach (var field in Fields)
            {
                if (!field.CheckSemantics(errors, warnings))
                    semantics_valid = false;
            }

            return semantics_valid;
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<Object>(expected);
            var expected_object = expected as Object;
            Assert.AreEqual(expected_object.Identifier, this.Identifier);
            Assert.AreEqual(expected_object.Fields.Count(), this.Fields.Count());
            for (int i = 0; i < this.Fields.Count; ++i)
                this.Fields[i].Validate(expected_object.Fields[i]);
        }
    }
}
