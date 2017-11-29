grammar BiPaGe;

Whitespace: [ \t\r\n\u000C]+ -> skip;
MultiLineComment: '/*' .*? '*/' -> skip;
SingleLineComment: '//' ~('\r' | '\n')* -> skip;

FieldType: (SizedField | FixedType);
SizedField: ('int' | 'uint' | 's' | 'u' )('0'..'9')('0'..'9')*;
FixedType: ( 'bool' | 'float32' | 'float64');
Collection: ('[' Identifier ']');

Identifier: ('a'..'z'|'A'..'Z'|'_')('a'..'z'|'A'..'Z'|'_'|'0'..'9')*;

file: object*;
object: Identifier '{' field+ '}';
field: Identifier ':' FieldType Collection? ';';
