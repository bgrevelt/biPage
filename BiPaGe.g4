grammar BiPaGe;

Whitespace: [ \t\r\n\u000C]+ -> skip;
MultiLineComment: '/*' .*? '*/' -> skip;
SingleLineComment: '//' ~('\r' | '\n')* -> skip;

FieldType: ('int' | 'uint' | 's' | 'u' | 'float' | 'bool' )('0'..'9')*;
NumberLiteral: ('0'..'9')('0'..'9')*;

//Collection: ('[' Identifier ']');

Identifier: ('a'..'z'|'A'..'Z'|'_')('a'..'z'|'A'..'Z'|'_'|'0'..'9')*;

file: object*;
object: Identifier '{' field+ '}';
field: name=Identifier ':' (basic_type=FieldType | complex_type=Identifier) multiplier? ';';
multiplier: '[' (NumberLiteral | Identifier ('.' Identifier)*) ']';
