using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    public class SizeChecker
    {
        private ExpressionResolver resolver = new ExpressionResolver();
        public bool HasStaticSize(Field f)
        {
            return false;
        }

        public bool HasStaticSize(FieldTypes.AsciiString s)
        {
            return resolver.IsStaticExpression(s.Size);
        }

        public bool HasStaticSize(FieldTypes.Boolean b)
        {
            return true;
        }

        public bool HasStaticSize(FieldTypes.Collection c)
        {
            return resolver.IsStaticExpression(c.Size);
        }

        public bool HasStaticSize(FieldTypes.FloatingPoint f)
        {
            return true;
        }

        public bool HasStaticSize(FieldTypes.Integral i)
        {
            return true;
        }

        public bool HasStaticSize(FieldTypes.Reference r)
        {
            /* TODO
             * A reference is a reference to a type (either an enumeration or a structure) We need to get the type from the name (so we require some sort of map for that)
             * and then we need to find out if the thing has a static size or not.
             * An enumeration always has a static size
             * A structure has a static size if all of it's fields are static.
             * */
            Dictionary<String, DataElement> data_elements = new Dictionary<string, DataElement>();
            return HasStaticSize((dynamic)data_elements[r.Name]);
        }

        public bool HasStaticSize(Enumeration e)
        {
            return true;
        }

        public bool HasStaticSize(Structure s)
        {
            foreach(var field in s.Fields)
            {
                if (!HasStaticSize(field))
                    return false;
            }
            return true;
        }
    }
}