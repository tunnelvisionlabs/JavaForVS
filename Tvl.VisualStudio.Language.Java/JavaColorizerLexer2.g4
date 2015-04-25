lexer grammar JavaColorizerLexer2;

tokens {
	DOC_COMMENT_INVALID_TAG
}

// separators
LPAREN:'(';
RPAREN:')';
LBRACKET:'[';
RBRACKET:']';
LBRACE:'{';
RBRACE:'}';
SEMI:';';
COMMA:',';
DOT:'.';
AT:'@';

// operators
EQ			:'=';
NEQ			:'!=';
EQEQ		:'==';
PLUS		:'+';
PLUSEQ		:'+=';
MINUS		:'-';
MINUSEQ		:'-=';
TIMES		:'*';
TIMESEQ		:'*=';
DIV			:'/';
DIVEQ		:'/=';
LT			:'<';
GT			:'>';
LE			:'<=';
GE			:'>=';
NOT			:'!';
BITNOT		:'~';
AND			:'&&';
BITAND		:'&';
ANDEQ		:'&=';
QUES		:'?';
OR			:'||';
BITOR		:'|';
OREQ		:'|=';
COLON		:':';
INC			:'++';
DEC			:'--';
XOR			:'^';
XOREQ		:'^=';
MOD			:'%';
MODEQ		:'%=';
LSHIFT		:'<<';
RSHIFT		:'>>';
LSHIFTEQ	:'<<=';
RSHIFTEQ	:'>>=';
ROR			:'>>>';
ROREQ		:'>>>=';

ABSTRACT
    :   'abstract'
    ;
    
ASSERT
    :   'assert' {AssertSupported}?
    ;
    
BOOLEAN
    :   'boolean'
    ;
    
BREAK
    :   'break'
    ;
    
BYTE
    :   'byte'
    ;
    
CASE
    :   'case'
    ;
    
CATCH
    :   'catch'
    ;
    
CHAR
    :   'char'
    ;
    
CLASS
    :   'class'
    ;
    
CONST
    :   'const'
    ;

CONTINUE
    :   'continue'
    ;

DEFAULT
    :   'default'
    ;

DO
    :   'do'
    ;

DOUBLE
    :   'double'
    ;

ELSE
    :   'else'
    ;

ENUM
    :   'enum' {EnumSupported}?
    ;             

EXTENDS
    :   'extends'
    ;

FINAL
    :   'final'
    ;

FINALLY
    :   'finally'
    ;

FLOAT
    :   'float'
    ;

FOR
    :   'for'
    ;

GOTO
    :   'goto'
    ;

IF
    :   'if'
    ;

IMPLEMENTS
    :   'implements'
    ;

IMPORT
    :   'import'
    ;

INSTANCEOF
    :   'instanceof'
    ;

INT
    :   'int'
    ;

INTERFACE
    :   'interface'
    ;

LONG
    :   'long'
    ;

NATIVE
    :   'native'
    ;

NEW
    :   'new'
    ;

PACKAGE
    :   'package'
    ;

PRIVATE
    :   'private'
    ;

PROTECTED
    :   'protected'
    ;

PUBLIC
    :   'public'
    ;

RETURN
    :   'return'
    ;

SHORT
    :   'short'
    ;

STATIC
    :   'static'
    ;

STRICTFP
    :   'strictfp'
    ;

SUPER
    :   'super'
    ;

SWITCH
    :   'switch'
    ;

SYNCHRONIZED
    :   'synchronized'
    ;

THIS
    :   'this'
    ;

THROW
    :   'throw'
    ;

THROWS
    :   'throws'
    ;

TRANSIENT
    :   'transient'
    ;

TRY
    :   'try'
    ;

VOID
    :   'void'
    ;

VOLATILE
    :   'volatile'
    ;

WHILE
    :   'while'
    ;

TRUE
    :   'true'
    ;

FALSE
    :   'false'
    ;

NULL
    :   'null'
    ;

IDENTIFIER
	:	('a'..'z' | 'A'..'Z' | '_' | '$')
		('a'..'z' | 'A'..'Z' | '0'..'9' | '_' | '$')*
	;

NUMBER
	:	(	'0'..'9'
		| 	'.' '0'..'9'
		)
		(IDENTIFIER NUMBER?)?
	;

COMMENT
	:	'//' (~('\r' | '\n'))*
	;

DOC_COMMENT_START
	:	'/**' -> pushMode(DocComment), type(DOC_COMMENT_TEXT)
	;

EMPTY_BLOCK_COMMENT
	:	'/**/' -> type(ML_COMMENT)
	;

ML_COMMENT
	:	'/*' -> pushMode(BlockComment)
	;

CHAR_LITERAL
	:	'\''
		(	'\\' ~[\r\n]?
		|	~['\r\n\\]
		)*
		'\''?
	;

STRING_LITERAL
	:	'"'
		(	'\\' ~[\r\n]?
		|	~["\r\n\\]
		)*
		'"'?
	;

WS
	:	[ \t]+
	;

NEWLINE
	:	'\r'? '\n'
	;

ANYCHAR
	:	.
	;

mode BlockComment;

	BlockComment_NEWLINE : NEWLINE -> type(NEWLINE);

	BlockComment_TEXT
		:	(	~[\r\n*]
			|	'*' ~[\r\n/]
			)+
			-> type(ML_COMMENT)
		;
	BlockComment_STAR : '*' -> type(ML_COMMENT);

	END_BLOCK_COMMENT : '*/' -> type(ML_COMMENT), popMode;

mode DocComment;

	DocComment_NEWLINE : NEWLINE -> type(NEWLINE);

	END_COMMENT
		:	'*/' -> type(DOC_COMMENT_TEXT), popMode
		;

	fragment // disables this rule for now without breaking references to the token type in code
	SVN_REFERENCE
		:	'$'
			(	~('$'|'\n'|'\r'|'*')
			|	{_input.La(2) != '/'}? '*'
			)*
			'$'
		;

	DOC_COMMENT_TEXT
		:	//'$'? // this is a stray '$' that couldn't be made into an SVN_REFERENCE
			(	~('@' | '\\' | '\r' | '\n' | '*' /*| '$'*/)
			|	{_input.La(2) != '/'}? '*'
			|	{!IsDocCommentStartCharacter(_input.La(2))}? ('\\' | '@')
			)+
		;

	DOC_COMMENT_TAG
		:	('\\' | '@')
			(	('$' | '@' | '&' | '~' | '<' | '>' | '#' | '%' | '"')
			|	'\\' '::'?
			|	'f' ('$' | '[' | ']' | '{' | '}')
			|	('a'..'z' | 'A'..'Z')+
			)
		;

	DocComment_ANYCHAR : ANYCHAR -> type(ANYCHAR);
