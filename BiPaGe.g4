grammar BiPaGe;

Whitespace: [ \t\r\n\u000C]+ -> skip;
MultiLineComment: '/*' .*? '*/' -> skip;
SingleLineComment: '//' ~('\r' | '\n')* -> skip;

BasicType: ('int' | 'uint' | 's' | 'u' | 'float' | 'bool' )('0'..'9')*;
NumberLiteral: ('0'..'9')('0'..'9')*;

//Collection: ('[' Identifier ']');

Identifier: ('a'..'z'|'A'..'Z'|'_')('a'..'z'|'A'..'Z'|'_'|'0'..'9')*;

objects: object*;
object: Identifier '{' field+ '}';
field: name=Identifier ':' fieldType ';';
fieldType: collection | singular;
collection: singular multiplier;
singular: (BasicType | Identifier);
multiplier: '[' (NumberLiteral | Identifier ('.' Identifier)*) ']';
