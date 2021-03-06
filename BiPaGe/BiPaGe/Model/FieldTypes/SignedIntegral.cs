﻿namespace BiPaGe.Model.FieldTypes
{
    public class SignedIntegral : Integral
    {
        public SignedIntegral(uint size) : base(size)
        {

        }

        public override bool HasStaticSize()
        {
            return true;
        }

        public override void Accept(IFieldTypeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
