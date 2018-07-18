using System;
using System.Collections.Generic;
using System.Linq;
namespace BiPaGe.Model.FieldTypes
{    
    public class Boolean : FieldType
    {
        public Boolean()
        {
        }

        public override bool HasStaticSize()
        {
            return true;
        }

        public override uint SizeInBits()
        {
            return 1;
        }
    }
}
