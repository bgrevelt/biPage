using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public interface IFieldTypeVisitor
    {
        void Visit(Model.FieldTypes.AsciiString s);
        void Visit(Model.FieldTypes.Boolean b);
        void Visit(Model.FieldTypes.Collection c);
        void Visit(Model.FieldTypes.FloatingPoint f);        
        void Visit(Model.FieldTypes.SignedIntegral s);
        void Visit(Model.FieldTypes.UnsignedIntegral u);
        void Visit(Model.Structure s);
        void Visit(Model.Enumeration e); 
    }
}
