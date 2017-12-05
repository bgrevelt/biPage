using System;
namespace BiPaGe.AST.Types.BasicTypes
{
    public class SizedType : BasicType
    {
        public UInt32 Size { get; } // The size of the integer field in bits

        public SizedType(String typeID)
        {
            this.Size = UInt32.Parse(typeID.TrimStart("abcdefghijklmnopqrstuvwxyz".ToCharArray()));
        }
    }
}
