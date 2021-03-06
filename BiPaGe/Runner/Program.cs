﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
collection_two : bool[(this + 5) / 3 ] = {true, false, true};
enum_field : Enumeration = value0;
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
            var static_input =
@"Object1 
{
field_one: int12;
field_two: u4;
}

Object2
{
field_three: uint18 = 50;
field_five: float64 = 1.23;
field_whatevs: bool = true;
field_bla: bool = false;
enum_field : Enumeration = value0;
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

            var dynamic_input =
@"Object1 
{
field_one: int12;
field_two: u4;
}

Object2
{
field_three: uint18;
field_four: int14;
field_five: float64;
dynamic_bool_collection: bool[field_three];
static_bool_collection: bool[(12-4) / 8];
enum_field : Enumeration;
anonimous1 : {
    field11 : i16;
    field12 : u32;
    anonimous2 : {
    field21 : f32;
    field22 : f64;

};
};
}

Enumeration : u8
{
    value0 = 0,
    value1 = 1,
    value2 = 2
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
            var lots_of_enums =
@"Color : u8
{
    Red = 0,
    Blue = 1,
    Green = 2,
    Brown = 4,
    White = 5,
    Yellow = 6
}

Tree 
{
    stem_width : f64;
    number_of_branches :u16;
    branches : 
    {
        branch_color : u8 { brown = 0, very_brown = 1, extremely_brown = 2, almost_black_but_still_brown = 3 };
        number_of_leaves : u16;
        leaves : 
        {
            width : f32;
            length : f32;
            color : u8 { green = 0, yellow = 1, orange = 2, red = 3, brown = 4};
        }[number_of_leaves];
    }[number_of_branches];
}";



            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();

            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var AST = builder.Program(dynamic_input);

            //bool valid = AST.CheckSemantics(errors, warnings);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ToString());
            }
            foreach (var warning in warnings)
            {
                Console.WriteLine(warning.ToString());
            }

            //AST.Print(0);
            var model_builder = new Model.Builder();
            model_builder.Build(AST);
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(model_builder.Enumerations, Formatting.Indented,  new JsonSerializerSettings() { ContractResolver = new MyContractResolver(), TypeNameHandling = TypeNameHandling.All }));
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(model_builder.Structures, Formatting.Indented, new JsonSerializerSettings() { ContractResolver = new MyContractResolver(), TypeNameHandling = TypeNameHandling.All }));

            var sw = new System.IO.StreamWriter(Console.OpenStandardOutput());
            sw.AutoFlush = true;
            BiPaGe.FrontEnd.CPP.CodeGenerator cg = new FrontEnd.CPP.CodeGenerator(model_builder.Enumerations, model_builder.Structures, sw);
            cg.Generate();
        }
    }

    public class MyContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Select(p => base.CreateProperty(p, memberSerialization))
                        .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                   .Select(f => base.CreateProperty(f, memberSerialization)))
                        .Where(p => !p.PropertyName.Contains("k__BackingField"))
                        .ToList();
            props.ForEach(p => { p.Writable = true; p.Readable = true; });
            return props;
        }
    }
}
