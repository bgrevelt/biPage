using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.SemanticAnalysis
{
    /* What ypes of errors do we need to catch here?
    - Fixer does not match fixed type
    -- Type mismatch field : int = 1.23
    -- identifier trouble. 
    --- Enum identifier needs to be of the right enum type
    --- Complex field initializer needs to be actually be a field in the type
    -- Size mismatch field : int[4] = { 1,2,3}, but also int4 = 255
    -- Fixers are not allowed after inline object or enum
    

    - Identifiers
    -- Object and or Enum needs to actually be part of the definition

    - Expressions
    -- Need to evaluate to integer (I think all of them do by default, but we may want to give a warning for divisions?)

    - Collections
    -- Either need to be sized, or have a fixer

    - Enumeration
    -- Check uniqueness of values
    -- Check uniqueness of identifiers
    -- Check that values match type 
    -- Check type (e.g. SomeEnum : f64 or SomeEnume : ascii_string

    - Complex types
    There's all sorts of things to check here. Basically all the fixer things should be checked again.
    Also there's nested things to think about:
    SuperNested
    { 
        field1 : int32;
        field2 : float64;
    }
    Nested
    {
        complex : SuperNested;
        someOtherField : bool[8];
    }
    Root
    {
        field1 : Nested(complex.field1 = 5)
    }

    */

    public class SemanticAnalyzer
    {
        public List<Event> CheckSemantics(AST.ASTNode program)
        {
            return new List<Event>();
        }
    }
}
