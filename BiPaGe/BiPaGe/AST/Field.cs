using System;
using System.Collections.Generic;

namespace BiPaGe.AST
{
    public class Field : IASTNode
    { 
        public String Name { get; }
        public AST.FieldType Type { get; }

        public Field(String name, AST.FieldType type)
        {
            this.Name = name;
            this.Type = type;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Field {0}: ", Name), indentLevel);
            Type.Print(indentLevel + 1);
        }

        public override bool CheckSemantics(IList<String> errors, IList<String> warnings)
        {
            return Type.CheckSemantics(errors, warnings);
        }
    }
}
