using System;

namespace BiPaGe.Model
{
    public class Enumerator 
    {
        public String Name { get; }
        public int Value { get; }

        public Enumerator(String name, int value)
        {
            this.Name = name;
            this.Value = value;
        }        
    }
}
