%defines
%tokentype ETokens

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

starting    :	dec_list EOF { Console.WriteLine("Ciao"); }
			;

dec_list	: 	declaration				{ $$ = new DeclarationStatementList($1); }
			| 	dec_list declaration	{ $1.statements.Add($2); $$ = $1; }
			;

declaration	:	ID opt_type IS expr SEMICOLON	{ $$ = new DeclarationStatement(new Identifier($1.values[0]), $2, $4); }
			;

opt_type	:	/* empty */		{ $$ = null; }
			|	COLON type		{ $$ = $1; }
			;

type		:	INTEGER			{ $$ = new IntegerType(); }
			|	COMPLEX			{ $$ = new ComplexType(); }
			|	RATIONAL		{ $$ = new RationalType(); }
			|	REAL			{ $$ = new RealType(); }
			|	STRING			{ $$ = new StringType(); }
			|	BOOLEAN			{ $$ = new BooleanType(); }
			|	func_type		{ $$ = $1; }
			|	tuple_type		{ $$ = $1; }
			|	array_type		{ $$ = $1; }
			|	map_type		{ $$ = $1; }
			;

expr		:	secondary							{ $$ = $1; }
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
			|	secondary ELLIPSIS secondary /* check */
			;

secondary	:	primary 					{ $$ = $1 }
			|	func_call
			|	secondary indexer
			;

primary		: 	value					{ $$ = $1; }
			|	cond					{ $$ = $1; }
			|	func_def				{ $$ = $1; }
			|	array_def				{ $$ = $1; }
			|	map_def					{ $$ = $1; }
			|	tuple_def				{ $$ = $1; }
//			|	LROUND expr RROUND
			;

value		:	BOOLEAN_VALUE			{ $$ = new BooleanValue((bool) $1.values[0]); }
			|	INTEGER_VALUE			{ $$ = new IntegerValue((int) $1.values[0]); }	
			|	REAL_VALUE				{ $$ = new RealValue((double) $1.values[0]); }
			|	RATIONAL_VALUE			{ $$ = new RationalValue((int) $1.values[0], (int) $1.values[1]); }
			|	COMPLEX_VALUE			{ $$ = new ComplexValue((double) $1.values[0], (double) $1.values[1]); }
			|	STRING_VALUE			{ $$ = new StringValue((string) $1.values[0]); }
			|	ID						{ $$ = new Identifier((string) $1.values[0]); }
			;

cond		:	IF expr THEN expr ELSE expr END		{ $$ = new Conditional($2, $4, $6); }
			;

func_def	:	FUNC LROUND opt_params RROUND opt_type func_body
			;

opt_params	:	/* empty */							{ $$ = null; }
			|	param_list							{ $$ = $1; }
			;

param_list	:	param 								{ $$ = $1; }
			| 	param_list COMMA param				{ $1.params.Add($3); $$ = $1; }
			;

param		:	ID COLON type 						{ $$ = new Parameter($1, $3); }
			;

func_body	:	DO stm_list END						{ $$ = $1; }
			|	ARROW LROUND expr RROUND
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
			;

opt_exprs	:	/* empty */
			|	expr_list
			;

expr_list	:	expr
			|	expr_list COMMA expr
			;

assignment	:	secondary ASSIGN expr SEMICOLON
			;

if_stm		:	IF expr THEN stm_list END 						{ if($1) { $2 } else {$3} }
			|	IF expr THEN stm_list ELSE stm_list END
			;

loop_stm	:	loop_header LOOP stm_list END
			;

loop_header	:	/* empty */
			|	FOR ID IN expr
			|	FOR expr
			|	WHILE expr
			;

return_stm	:	RETURN SEMICOLON				/* possible fix: recognize new line before END token */
			|	RETURN expr SEMICOLON
			;

break_stm	:	BREAK SEMICOLON
			;

cont_stm	:	CONTINUE SEMICOLON
			;

print_stm	:	PRINT LROUND opt_exprs RROUND SEMICOLON
			;

array_def	:	LSQUARE opt_exprs RSQUARE 
			;

map_def		:	LCURLY pair_list RCURLY 
			;

pair_list	:	/* empty */
			|	pair
			|	pair_list COMMA pair
			;

pair		:	expr COLON expr 
			;

tuple_def	:	LROUND tuple_elist RROUND 
			;

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

func_type	:	FUNC LROUND type_list RROUND COLON type 
			;

type_list	:	type
			|	type_list COMMA type
			;

array_type	:	LSQUARE type RSQUARE 
			;

tuple_type	: 	LROUND type_list RROUND
			;

map_type	:	LCURLY type COLON type RCURLY 
			;
