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
    }
}
