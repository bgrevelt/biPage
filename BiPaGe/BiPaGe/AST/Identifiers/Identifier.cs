using System;
using System.Collections.Generic;

namespace BiPaGe.AST.Identifiers
{
    public class Identifier : AST.FieldType, Constants.Constant
    {
        public String Id { get; }
        public Identifier(SourceInfo sourceIfo, String id) : base(sourceIfo)
        {
            this.Id = id;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Object id: {0}", Id), indentLevel);
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            // TODO: we should pass in a list of know objects and their fields
            return true;
        }

        public override bool Equals(FieldType other)
        {
            try
            {
                return ((Identifier)other).Id == this.Id;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}
