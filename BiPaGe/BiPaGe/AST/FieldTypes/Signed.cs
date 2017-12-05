using System;
using System.Collections.Generic;

namespace BiPaGe.AST.FieldTypes
{
    public class Signed : SizedType
    {
        public Signed(String typeId) : base(typeId)
        {
        }

        public override bool CheckSemantics(IList<string> errors, IList<string> warnings)
        {
            // Support integer lengths of up to 64 bits for now. We may want to support 128 bits in the future, but most
            // programming languages do not support integers that long
            if(Size > 64 || Size < 2)
            {
                errors.Add(String.Format("Unsupported signed integer width ({0}). Signed integers widths in the range [2,64] are supported.", Size));
                return false;
            }
            return true;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("{0} bit signed integer", Size), indentLevel);
        }
    }
}
