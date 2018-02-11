using System;
using System.Collections.Generic;

namespace BiPaGe.AST.FieldTypes
{
    public class Unsigned : SizedType
    {
        public Unsigned(SourceInfo sourceInfo, int size) : base(sourceInfo, size)
        {
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            // Support integer lengths of up to 64 bits for now. We may want to support 128 bits in the future, but most
            // programming languages do not support integers that long
            if (Size == 1)
            {
                warnings.Add(new SemanticAnalysis.Warning(sourceInfo, "Consider using a boolean field instead of a one bit integer field."));
                return false;
            }
            if (Size > 64 || Size < 1)
            {
                errors.Add(new SemanticAnalysis.Error(sourceInfo, String.Format("Unsupported unsigned integer width ({0}). Singed integers widths in the range [1,64] are supported.", Size)));
                return false;
            }
            return true;
        }

        public override bool Equals(IASTNode other)
        {
            try
            {
                return ((Unsigned)other).Size == this.Size;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("{0} bit unsigned integer", Size), indentLevel);
        }
    }
}
