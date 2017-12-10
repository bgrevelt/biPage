using System;
namespace BiPaGe.SemanticAnalysis
{
    public class Error : Event
    {
        public Error(AST.SourceInfo sourceInfo, String message) : base(sourceInfo, message)
        {
        }

        public override String ToString()
        {
            return base.ToString("Error");
        }
    }
}
