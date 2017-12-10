using System;
namespace BiPaGe.SemanticAnalysis
{
    public class Warning : Event
    {
        public Warning(AST.SourceInfo sourceInfo, String message) : base(sourceInfo, message)
        {
        }

        public override String ToString()
        {
            return base.ToString("Warning");
        }
    }
}
