using System;
namespace BiPaGe.SemanticAnalysis
{
    public class Event
    {
        private AST.SourceInfo sourceInfo; // TODO: either this class belongs in the AST or SourceInfo belongs outside of the AST namespace 
        private String message;
        public Event(AST.SourceInfo sourceInfo, String message)
        {
            this.sourceInfo = sourceInfo;
            this.message = message;
        }

        protected String ToString(String prefix)
        {
            return String.Format("SomeFile.bla({2}:{3}): {0}: {1} ", prefix, this.message, sourceInfo.line, sourceInfo.column);
        }
    }
}
