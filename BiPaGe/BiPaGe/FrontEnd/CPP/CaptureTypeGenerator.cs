using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiPaGe.Model;
using BiPaGe.Model.FieldTypes;

namespace BiPaGe.FrontEnd.CPP
{
    class CaptureTypeGenerator : Model.IFieldTypeVisitor
    {
        private readonly FieldTypeConverter Converter = new FieldTypeConverter();
        private string Generated;
        private uint Size;
        private bool NeedsBitMangling;
        public String Generate(Model.FieldType type, uint size, bool needsBitMangling)
        {
            this.Generated = null;
            this.Size = size;
            this.NeedsBitMangling = needsBitMangling;
            type.Accept(this);
            Debug.Assert(this.Generated != null);
            return this.Generated;
        }

        public void Visit(AsciiString s)
        {
            throw new NotImplementedException();
        }

        public void Visit(Model.FieldTypes.Boolean b)
        {
            throw new NotImplementedException();
        }

        public void Visit(Collection c)
        {
            throw new NotImplementedException();
        }

        public void Visit(FloatingPoint f)
        {            
            Debug.Assert(this.Size == 32 || this.Size == 64);
            if (this.NeedsBitMangling)
            {
                // Floating point types are captured as uints if we need to perofrm bit manipulation on them (e.g. masking, shifting)
                this.Generated = String.Format("std::uint{0}_t", this.Size);
            }
            else if(this.Size == 32)
            {
                this.Generated = "float";
            }
            else
            {
                this.Generated = "double";
            }
        }

        public void Visit(SignedIntegral s)
        {
            this.Generated = String.Format("std::int{0}_t", this.Size);
        }

        public void Visit(UnsignedIntegral u)
        {
            this.Generated = String.Format("std::uint{0}_t", this.Size);
        }

        public void Visit(Structure s)
        {
            throw new NotImplementedException();
        }

        public void Visit(Enumeration e)
        {
            if (this.NeedsBitMangling)
            {
                // If we need to do some bit mangling, first convert to the underlying type (I don't think we really need to do that, but it is more clear then performing bitwise operations
                // on an enumeraiton
                e.Type.Accept(this);
            }
            else
            {
                this.Generated = e.Name;
            }               
        }
    }
}
