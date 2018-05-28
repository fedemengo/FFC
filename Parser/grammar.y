%error-verbose
%token ID BOOLEAN_VALUE INTEGER_VALUE REAL_VALUE RATIONAL_VALUE COMPLEX_VALUE STRING_VALUE
%token DOT COMMA COLON SEMICOLON
%token STAR SLASH
%token PLUS MINUS

%token INTEGER COMPLEX RATIONAL REAL STRING BOOLEAN

%token ASSIGN ARROW
%token LESS LESSEQUAL GREATER GREATEREQUAL EQUAL NOTEQUAL
%token AND OR XOR

%token FUNC DO RETURN PRINT
%token IS IF THEN ELSE END
%token WHILE FOR IN LOOP BREAK CONTINUE
%token LROUND RROUND LSQUARE RSQUARE LCURLY RCURLY
%token ELLIPSIS

%token EOF

%start starting

%left LESS LESSEQUAL GREATER GREATEREQUAL EQUAL NOTEQUAL
%left AND OR XOR
%left PLUS MINUS
%left STAR SLASH
%left NEG

%%

starting    :	dec_list EOF ;

dec_list	: 	declaration
			| 	declaration dec_list
			;

declaration	:	ID opt_type IS expr SEMICOLON ;

opt_type	:	/* empty */
			|	COLON type
			;

type		:	INTEGER
			|	COMPLEX
			|	RATIONAL
			|	REAL
			|	STRING
			|	BOOLEAN
			|	func_type
			|	tuple_type
			|	array_type
			|	map_type
			;

expr		:	secondary
			|	secondary LESS expr
			|	secondary LESSEQUAL expr
			|	secondary GREATER expr
			|	secondary GREATEREQUAL expr
			|	secondary EQUAL expr
			|	secondary NOTEQUAL expr
			|	secondary AND expr
			|	secondary OR expr
			|	secondary XOR expr
			|	secondary PLUS expr
			|	secondary MINUS expr
			|	secondary STAR expr
			|	secondary SLASH expr
			|	MINUS secondary NEG
			|	secondary ELLIPSIS secondary /* shall it just be a special case for FOR loops or a whole new type? */ //causes a hell of errors
			;

secondary	:	primary
			|	func_call
			|	secondary indexer
			;

primary		: 	value
			|	cond
			|	func_def /* lot of shift reduce */
			|	array_def
			|	map_def
			|	tuple_def
			|	LROUND expr RROUND
			;

value		:	BOOLEAN_VALUE
			|	INTEGER_VALUE
			|	REAL_VALUE
			|	RATIONAL_VALUE
			|	COMPLEX_VALUE
			|	STRING_VALUE
			|	ID
			;

cond		:	IF expr THEN expr ELSE expr END ;

func_def	:	FUNC LROUND opt_params RROUND opt_type func_body ;

opt_params	:	/* empty */
			|	param_list
			;

param_list	:	param
			| 	param_list COMMA param
			;

param		:	ID COLON type ;

func_body	:	DO stm_list END
			|	ARROW LROUND expr RROUND	/* expression within () */
			;

stm_list	:	statement
			|	stm_list statement
			;

statement	:	func_call SEMICOLON
			|	assignment
			|	declaration
			|	if_stm
			|	loop_stm
			|	return_stm
			|	break_stm
			|	cont_stm
			|	print_stm
			;

func_call	:	secondary LROUND opt_exprs RROUND

opt_exprs	:	/* empty */
			|	expr_list
			;

expr_list	:	expr
			|	expr_list COMMA expr
			;

assignment	:	secondary ASSIGN expr SEMICOLON ;

if_stm		:	IF expr THEN stm_list END
			|	IF expr THEN stm_list ELSE stm_list END
			;

loop_stm	:	loop_header LOOP stm_list END ;

loop_header	:	/* empty */
			|	FOR ID IN expr
			|	FOR expr
			|	WHILE expr
			;

return_stm	:	RETURN SEMICOLON
			|	RETURN expr SEMICOLON
			;

break_stm	:	BREAK SEMICOLON ;

cont_stm	:	CONTINUE SEMICOLON ;

print_stm	:	PRINT LROUND opt_exprs RROUND SEMICOLON ;

array_def	:	LSQUARE opt_exprs RSQUARE

map_def		:	LCURLY pair_list RCURLY ;

pair_list	:	/* empty */
			|	pair
			|	pair_list COMMA pair
			;

pair		:	expr COLON expr ;

tuple_def	:	LROUND tuple_elist RROUND ;

tuple_elist :	tuple_elem
			|	tuple_elist COMMA tuple_elem
			;

tuple_elem	:	ID IS expr
			|	expr
			;

indexer		:	LSQUARE expr RSQUARE
			|	DOT	ID
			|	DOT INTEGER_VALUE
			;

func_type	:	FUNC LROUND type_list RROUND COLON type ;

type_list	:	type
			|	type_list COMMA type
			;

array_type	:	LSQUARE type RSQUARE ; /* can be the same as array def */

tuple_type	: 	LROUND type_list RROUND ; /* tuple properties name during type definition ? */

map_type	:	LCURLY type COLON type RCURLY ;