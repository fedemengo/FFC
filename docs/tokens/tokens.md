## F tokens

### keywords:
	IS -> is
	
	IF -> if
	THEN -> then
	ELSE -> else
	END -> end
	
	WHILE -> while
	FOR -> for
	IN -> in
	LOOP -> loop
	BREAK -> break

	FUNC -> func
	DO -> do
	RETURN -> return
	
	PRINT -> print	

### types:
		
	INTEGER -> integer
	COMPLEX -> complex 
	RATIONAL -> rational 
	REAL -> real 
	STRING -> string 
	BOOLEAN -> boolean

### punctuation:

	ASSIGN -> :=
	ARROW -> =>

	DOT -> .
	COMMA -> ,
	COLON -> :
	SEMICOLON -> ;

	PLUS -> +
	MINUS -> -
	STAR -> *
	SLASH -> /

	LESS -> <
	LESSEQUAL -> <=
	GREATER -> >
	GREATEREQUAL -> >=
	EQUAL -> =
	NOTEQUAL -> /=

	AND -> &
	OR -> |
	XOR -> ^

### parenthesis:
	LROUND -> (
	RROUND -> )
	LSQUARE -> [
	RSQUARE -> ]
	LCURLY -> {
	RCURLY -> }

### other 

	BOOLEAN_VALUE(value) -> true | false
	INTEGER_VALUE(value) -> [0-9]+
	REAL_VALUE(value) -> [0-9]+.[0-9]+
	RATIONAL_VALUE(num, den) -> [0-9]+\[0-9]+
	COMPLEX_VALUE(real, img) -> [0-9]+.[0-9]+i[0-9]+.[0-9]+
	STRING_VALUE(val) ->"[ !#-~]"
		", then everything but ", then again another "
	ID("variable_name") -> [a-zA-Z][a-zA-Z0-9]*
