using System;
using System.Collections.Generic;

namespace BiPaGe.AST
{
    public class Object : IASTNode
    {
        public String identifier
        {
            get;
        }

        public IEnumerable<Field> fields
        {
            get;
        }

        public Object(SourceInfo sourceInfo, String identifier, IEnumerable<Field> fields) : base(sourceInfo)
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
    }
}
