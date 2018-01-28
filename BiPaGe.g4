grammar BiPaGe;

// Lexer rules
Whitespace: [ \t\r\n\u000C]+ -> skip;
MultiLineComment: '/*' .*? '*/' -> skip;
SingleLineComment: '//' ~('\r' | '\n')* -> skip;

Type: (('int' | 'uint' | 'float' | 'i' | 'u' | 'f') NumberLiteral) | 'bool' | 'ascii_string' | 'utf8_string';
This: 'this';
NumberLiteral: [0-9]+;
FloatLiteral: [0-9]+ '.' [0-9]+;
StringLiteral: '"' (~'"')* '"';
BooleanLiteral: 'true' | 'false';
Identifier: ('a'..'z'|'A'..'Z'|'_')('a'..'z'|'A'..'Z'|'_'|'0'..'9')*;


// Parser rules
program: element*;
element: object | enumeration;
enumeration: Identifier ':' Type '{' (enumerator ',')* enumerator '}';
enumerator: Identifier '=' NumberLiteral;
object: Identifier '{' field+ '}';
field:
  (Identifier ':')?
  field_type
  ('[' expression ']')?
  fixer?
  ';';

field_type: (Type | Identifier | inline_enumeration | inline_object);

inline_enumeration : Type '{' (enumerator ',')* enumerator '}';
inline_object: '{' field+ '}';

fixer: field_constant | object_constant;
object_constant: '(' (field_id '=' constant ','?)+ ')';
field_constant: '=' constant;

/*
initialization_value:
  NumberLiteral #NumberLiteral
  | FloatLiteral #FloatLiteral
  | StringLiteral #StringLiteral
  | BooleanLiteral #BooleanLiteral
  | Identifier #ObjectId
  | '{' (NumberLiteral ',')* NumberLiteral'}' #NumberCollection
  | '{' (FloatLiteral ',')* FloatLiteral'}' #FloatCollection
  | '{' (BooleanLiteral ',')* BooleanLiteral'}' #BooleanCollection;
*/


literal: NumberLiteral | FloatLiteral | StringLiteral | BooleanLiteral;

constant:
  literal #LiteralConstant
  | Identifier #ObjectId // enumerand
  | '{' (literal ',')* literal'}' #NumberCollection;

expression:
    NumberLiteral #Number
    | This #Offset
    | field_id #FieldId
    |'(' expression ')' #Parentheses
        | left=expression op='*' right=expression #BinaryOperation
        | left=expression op='/' right=expression #BinaryOperation
		| left=expression op='+' right=expression #BinaryOperation
		| left=expression op='-' right=expression #BinaryOperation;

field_id : Identifier ('.' Identifier)*;
