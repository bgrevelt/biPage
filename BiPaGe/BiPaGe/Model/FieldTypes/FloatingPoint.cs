namespace BiPaGe.Model.FieldTypes
{
    public class FloatingPoint : FieldType
    {
        public uint Size { get; } 
        public FloatingPoint(uint size)
        {
            this.Size = size;
        }

        public override bool HasStaticSize()
        {
            return true;
        }

        public override uint SizeInBits()
        {
            return this.Size;
        }
    }
}
