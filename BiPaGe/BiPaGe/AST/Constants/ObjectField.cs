using System;
namespace BiPaGe.AST.Constants
{
    public class ObjectField
    {
        public String FieldId { get; }
        public Value Value { get; }

        public ObjectField(String field_id, Value value)
        {
            this.FieldId = field_id;
            this.Value = value;
        }
    }
}
