using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BiPaGe.AST
{
    public class Object : Element
    {
        public String identifier
        {
            get;
        }

        public IList<Field> fields
        {
            get;
        }

        public Object(SourceInfo sourceInfo, String identifier, IList<Field> fields) : base(sourceInfo)
        {
            this.identifier = identifier;
            this.fields = fields;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Object {0}: ", identifier), indentLevel);

            foreach (var field in fields)
            {
                field.Print(indentLevel + 1);
            }
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            bool semantics_valid = true;

            foreach (var field in fields)
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
            Assert.AreEqual(expected_object.identifier, this.identifier);
            Assert.AreEqual(expected_object.fields.Count(), this.fields.Count());
            for (int i = 0; i < this.fields.Count; ++i)
                this.fields[i].Validate(expected_object.fields[i]);
        }
    }
}
