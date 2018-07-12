using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.Expressions
{
    public class Number : Expression
    {
        int Value { get; }
        public Number(int value)
        {
            this.Value = value;
        }
    }
}
