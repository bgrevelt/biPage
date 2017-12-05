using System;
namespace BiPaGe.AST.Types
{
    public class FieldIdentifier : IMultiplier
    {
        private String id;
        public FieldIdentifier(String id)
        {
            this.id = id;
        }
    }
}
