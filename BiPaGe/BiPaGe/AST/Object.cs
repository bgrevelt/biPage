using System;
using System.Collections.Generic;
using System.Linq;
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

        public override bool Equals(IASTNode other)
        {
            var other_object = other as Object;

            if (this.identifier != other_object.identifier)
                return false;

            if (this.fields.Count != other_object.fields.Count)
                return false;

            if (this.fields.Zip(other_object.fields, (l, r) => l.Equals(r)).Any(v => v == false))
                return false;

            return true;

        }
    }
}
