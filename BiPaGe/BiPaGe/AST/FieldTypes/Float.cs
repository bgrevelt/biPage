using System;
using System.Collections.Generic;

namespace BiPaGe.AST.FieldTypes
{
    public class Float : SizedType
    {
        public Float(SourceInfo sourceInfo, String typeId) : base(sourceInfo, typeId)
        {
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            if(Size != 32 && Size != 64)
            {
                errors.Add(new SemanticAnalysis.Error(sourceInfo, String.Format("Size {0} not supported for floating point type. Only float32 and float64 are supported", Size)));
                return false;
            }

            return true;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("{0} bit float", Size), indentLevel);
        }
    }
}
