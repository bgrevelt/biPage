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
    public class FieldTypeConverter : Model.IFieldTypeVisitor
    {
        private String converted = null;
        private uint? size = null;
        public String Convert(Model.FieldType type, uint? size = null)
        {
            converted = null;
            this.size = size;
            type.Accept(this);
            Debug.Assert(converted != null);
            return converted;
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
            var size = this.size ?? f.Size;
            Debug.Assert(size == 32 || size == 64);
            if (size == 32)
                converted = "float";
            else
                converted = "double";
        }

        public void Visit(SignedIntegral s)
        {
            var size = this.size ?? s.size;
            var cPP_type_size = (uint)Math.Max(8, Math.Pow(2, Math.Ceiling(Math.Log(size) / Math.Log(2))));
            Debug.Assert(cPP_type_size % 8 == 0);
            converted =  String.Format("std::int{0}_t", cPP_type_size);
        }

        public void Visit(UnsignedIntegral u)
        {
            var size = this.size ?? u.size;
            var cPP_type_size = (uint)Math.Max(8, Math.Pow(2, Math.Ceiling(Math.Log(size) / Math.Log(2))));
            Debug.Assert(cPP_type_size % 8 == 0);
            converted = String.Format("std::uint{0}_t", cPP_type_size);
        }

        public void Visit(Structure s)
        {
            throw new NotImplementedException();
        }

        public void Visit(Enumeration e)
        {
            converted = e.Name;
        }
    }
}
