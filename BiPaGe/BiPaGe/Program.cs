using System;
using Antlr4.Runtime;

namespace BiPaGe
{
    class Test : BiPaGeBaseVisitor<int>
    {
        private int indent = 0;

        public override int VisitFile(BiPaGeParser.FileContext context)
        {
            Console.WriteLine("File");
            indent++;
            foreach (var obj in context.@object())
            {
                obj.Accept(this);
            }
            indent--;
            return 0;
        }

        private void do_indent()
        {
            for (int i = 0; i < indent; ++i)
                Console.Write('\t');
        }

        public override int VisitObject(BiPaGeParser.ObjectContext context)
        {
            do_indent();
            Console.WriteLine("Object: " + context.Identifier().GetText());
            indent++;
            foreach (var field in context.field())
            {
                field.Accept(this);
            }
            indent--;
            return 0;
        }

        public override int VisitField(BiPaGeParser.FieldContext context)
        {
            do_indent();
            var type = "";
            if (context.basic_type != null)
                type = context.basic_type.Text;
            else
                type = context.complex_type.Text;
            Console.WriteLine("Field! " + context.name.Text + ", " + type);
            indent++;
            if (context.multiplier() != null)
                context.multiplier().Accept(this);
            indent--;
            return 0;
        }

        public override int VisitMultiplier(BiPaGeParser.MultiplierContext context)
        {
            do_indent();
            Console.Write("Multiplier! ");
            if (context.Identifier().Length != 0)
            {
                foreach (var id in context.Identifier())
                    Console.Write(id.GetText() + ".");
            }
            else
            {
                Console.Write(context.NumberLiteral().GetText());
            }

            Console.WriteLine();
            return 0;
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            var input =
@"Object1 
{
field_one: int12;
field_two: u4;
}

Object2
{
field_three: uint18;
field_four: s14;
field_five: float64;
}

Object3
{
field_six: float32;
field_seven: bool;
}

ObjectWithCollections
{
  size_field : uint16;
  embedded : Object2;
  collection_one : int32[5]; // hard coded size
  collection_two : bool[size_field]; // sized from field
  collection_three : float64[embedded.field_three];
}
";

            var inputStream = new AntlrInputStream(input);
            var lexer = new BiPaGeLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new BiPaGeParser(tokens);

            var f = parser.file();

            var test = new Test();
            test.Visit(f);
        }


    }
}
