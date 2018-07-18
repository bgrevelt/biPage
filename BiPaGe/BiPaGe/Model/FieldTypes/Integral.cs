namespace BiPaGe.Model.FieldTypes
{
    public abstract class Integral : FieldType
    {
        public Integral(uint size)
        {
            this.size = size;
        }
        public uint size;   // size in bits

        public override uint SizeInBits()
        {
            return this.size;
        }
    }
}
