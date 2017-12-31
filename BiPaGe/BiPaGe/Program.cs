using System;
using System.Collections.Generic;

namespace BiPaGe
{
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
field_six: f32;
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
            var invalid_input =
@"Object1 
{
field_one: int1;
field_two: u1;
}

Object2
{
field_three: uint18;
field_four: s0;
field_five: float12;
}

Object3
{
field_six, flaot49;
field_seven: bool5;
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
           

            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();

            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var AST = builder.Objects(invalid_input);

            bool valid = AST.CheckSemantics(errors, warnings);
            foreach(var error in errors)
            {
                Console.WriteLine(error.ToString());
            }
            foreach (var warning in warnings)
            {
                Console.WriteLine(warning.ToString());
            }
            if(valid)
                AST.Print(0);
        }


    }
}
