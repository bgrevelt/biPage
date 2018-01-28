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
field_one: int12[5];
field_two: u4;
}

Object2
{
field_three: uint18 = 50;
field_four: ascii_string[10] = ""Test"";
field_five: float64 = 1.23;
field_whatevs: bool = true;
field_bla: bool = false;
collection_two : bool[(this + 5) / 3 ];
}

Enumeration : u8
{
    value0 = 0,
    value1 = 1,
    value2 = 2
}

/*
{
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
}*/
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
            var AST = builder.Program(input);

            //bool valid = AST.CheckSemantics(errors, warnings);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ToString());
            }
            foreach (var warning in warnings)
            {
                Console.WriteLine(warning.ToString());
            }
            //if (valid)
            //{
                AST.Print(0); 
            //}
        }
        /*
        Name":"No name yet",
        "Objects":
        [
            {
                "identifier":"Object1",
                "fields":
                [
                    {
                        "Name":"field_one",
                        "Type":
                        {
                            "Size":12,
                            "sourceInfo":{"line":3,"column":11}
                        },
                        "sourceInfo":{"line":3,"column":0}
                     },
                     {
                        "Name":"field_two",
                        "Type":
                        {
                            "Size":4,
                            "sourceInfo":{"line":4,"column":11}
                        },
                        "sourceInfo":{"line":4,"column":0}
                     }

                ],
                "sourceInfo":{"line":1,"column":0}
              },{"identifier":"Object2","fields":[{"Name":"field_three","Type":{"Size":18,"sourceInfo":{"line":9,"column":13}},"sourceInfo":{"line":9,"column":0}},{"Name":"field_four","Type":{"Size":14,"sourceInfo":{"line":10,"column":12}},"sourceInfo":{"line":10,"column":0}},{"Name":"field_five","Type":{"Size":64,"sourceInfo":{"line":11,"column":12}},"sourceInfo":{"line":11,"column":0}}],"sourceInfo":{"line":7,"column":0}}],"sourceInfo":{"line":1,"column":0}}

        */
    }
}
