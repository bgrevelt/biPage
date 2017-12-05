using System;
namespace BiPaGe.AST
{
    public class Field : IASTNode
    { 
        public String Name { get; }
        public AST.Types.Type Type { get; }

        public Field(String name, AST.Types.Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Field {0}: ", Name), indentLevel);
            Type.Print(indentLevel + 1);
        }
    }
}
