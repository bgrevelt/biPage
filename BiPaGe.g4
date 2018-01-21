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
  initializer?
  ';';

field_type: (Type | Identifier | inline_enumeration | inline_object);

inline_enumeration : Type '{' (enumerator ',')* enumerator '}';
inline_object: '{' field+ '}';

initializer: standard_initializer |complex_initializer;
complex_initializer: '(' (field_id '=' initialization_value ','?)+ ')';
standard_initializer: '=' initialization_value;

initialization_value:
  NumberLiteral
  | FloatLiteral
  | StringLiteral
  | BooleanLiteral
  | Identifier // enumerator
  | '{' (NumberLiteral ',')* NumberLiteral'}'
  | '{' (FloatLiteral ',')* FloatLiteral'}'
  | '{' (BooleanLiteral ',')* BooleanLiteral'}';

expression:
    NumberLiteral
    | This
    | field_id
    |'(' expression ')'
		| expression '+' expression
		| expression '-' expression
		| expression '*' expression
		| expression '/' expression;

field_id : Identifier ('.' Identifier)*;
