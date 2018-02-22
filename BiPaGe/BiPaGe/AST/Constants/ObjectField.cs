using System;
namespace BiPaGe.AST.Constants
{
    public class ObjectField
    {
        public String FieldId { get; }
        public IFixer Value { get; }

        public ObjectField(String field_id, IFixer value)
        {
            this.FieldId = field_id;
            this.Value = value;
        }
    }
}
