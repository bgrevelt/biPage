grammar BiPaGe;

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

program: (object | enumeration)*;
enumeration: Identifier ':' Type '{' enumerator+ '}';
enumerator: Identifier '=' NumberLiteral ','?;
object: Identifier '{' (field | reserved_field)+ '}';
reserved_field: field_type ';';
field: (standard_field | fixed_field | inline_field ) ';';

standard_field: simple_field | complex_field;
simple_field: Identifier ':' field_type;
complex_field: Identifier ':' Identifier;

fixed_field: fixed_simple_field | fixed_complex_field | fixed_enum_field;
fixed_simple_field: simple_field '=' initializer;
fixed_enum_field: complex_field '=' Identifier;
fixed_complex_field: complex_field '(' (field_id '=' initializer ','?)+ ')';

initializer:
  NumberLiteral
  | FloatLiteral
  | StringLiteral
  | BooleanLiteral
  | Identifier // enumerator
  | '{' (NumberLiteral ',')* NumberLiteral'}'
  | '{' (FloatLiteral ',')* FloatLiteral'}'
  | '{' (BooleanLiteral ',')* BooleanLiteral'}';

inline_field: inline_enumeration | inline_object;
inline_enumeration : Identifier ':' Type '{' enumerator+ '}';
inline_object: Identifier ':' '{' (field | reserved_field)+ '}';
field_type: Type ('[' expression ']')?;
field_id : Identifier ('.' Identifier)*;
expression: NumberLiteral
    | This
    | field_id
    |'(' expression ')'
		| expression '+' expression
		| expression '-' expression
		| expression '*' expression
		| expression '/' expression;
