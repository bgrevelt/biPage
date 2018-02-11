using System;
namespace BiPaGe.AST.Constants
{
    public class ObjectField
    {
        public String FieldId { get; }
        public Constant Value { get; }

        public ObjectField(String field_id, Constant value)
        {
            this.FieldId = field_id;
            this.Value = value;
        }
    }
}
