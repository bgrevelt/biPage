namespace BiPaGe.Model.FieldTypes
{
    public class UnsignedIntegral : Integral
    {
        public UnsignedIntegral(uint size) : base(size)
        {

        }

        public override bool HasStaticSize()
        {
            return true;
            
        }
    }
}
