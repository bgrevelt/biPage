using System;

namespace BiPaGe.Model
{
    public abstract class DataElement : FieldType
    {
        public String Name { get; }
        public DataElement(String name)
        {
            this.Name = name;
        }
    }
}
