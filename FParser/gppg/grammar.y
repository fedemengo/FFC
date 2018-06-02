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

%YYSTYPE TValue

%%


starting    :	dec_list EOF { $$ = $1; }
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
			|	secondary LESS expr					{ $$ = new BinaryOperatorExpression($1, new LessOperator(), $3); }
			|	secondary LESSEQUAL expr			{ $$ = new BinaryOperatorExpression($1, new LessEqualOperator(), $3); }	
			|	secondary GREATER expr				{ $$ = new BinaryOperatorExpression($1, new GreaterOperator(), $3); }
			|	secondary GREATEREQUAL expr			{ $$ = new BinaryOperatorExpression($1, new GreaterEqualOperator(), $3); }
			|	secondary EQUAL expr				{ $$ = new BinaryOperatorExpression($1, new EqualOperator(), $3); }
			|	secondary NOTEQUAL expr				{ $$ = new BinaryOperatorExpression($1, new NotEqualOperator(), $3); }
			|	secondary AND expr					{ $$ = new BinaryOperatorExpression($1, new AndOperator(), $3); }
			|	secondary OR expr					{ $$ = new BinaryOperatorExpression($1, new OrOperator(), $3); }
			|	secondary XOR expr					{ $$ = new BinaryOperatorExpression($1, new XorOperator(), $3); }
			|	secondary PLUS expr					{ $$ = new BinaryOperatorExpression($1, new PlusOperator(), $3); }
			|	secondary MINUS expr				{ $$ = new BinaryOperatorExpression($1, new MinusOperator(), $3); }
			|	secondary STAR expr					{ $$ = new BinaryOperatorExpression($1, new StarOperator(), $3); }
			|	secondary SLASH expr				{ $$ = new BinaryOperatorExpression($1, new SlashOperator(), $3); }
			|	MINUS secondary NEG					{ $$ = new NegativeExpression($2); }
			|	secondary ELLIPSIS secondary 		{ $$ = new EllipsisExpression($1, $3); }
			;

secondary	:	primary 					{ $$ = $1; }
			|	func_call					{ $$ = $1; }
			|	secondary indexer			{ $$ = new IndexedAccess($1, $2); }
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

func_def	:	FUNC LROUND opt_params RROUND opt_type func_body		{ $$ = new FunctionDefinition($3, $5, $6); }
			;

opt_params	:	/* empty */							{ $$ = new ParameterList(); }
			|	param_list							{ $$ = $1; }
			;

param_list	:	param 								{ $$ = new ParamenterList($1); }
			| 	param_list COMMA param				{ $1.parameters.Add($3); $$ = $1; }
			;

param		:	ID COLON type 						{ $$ = new Parameter($1, $3); }
			;

func_body	:	DO stm_list END						{ $$ = $1; }
			|	ARROW LROUND expr RROUND			{ $$ = new StatementList(new ExpressionStatement($3)); }
			;

stm_list	:	statement							{ $$ = new StatementList($1); }
			|	stm_list statement					{ $1.statements.Add($2); $$ = $1; }
			;

statement	:	func_call SEMICOLON					{ $$ = $1; }
			|	assignment							{ $$ = $1; }
			|	declaration							{ $$ = $1; }
			|	if_stm								{ $$ = $1; }
			|	loop_stm							{ $$ = $1; }
			|	return_stm							{ $$ = $1; }
			|	break_stm							{ $$ = $1; }
			|	cont_stm							{ $$ = $1; }
			|	print_stm							{ $$ = $1; }
			;

func_call	:	secondary LROUND opt_exprs RROUND	{ $$ = new FunctionCall($1, $3); }
			;

opt_exprs	:	/* empty */							{ $$ = new ExpressionList(); }
			|	expr_list							{ $$ = $1; }
			;

expr_list	:	expr								{ $$ = new ExpressionList($1); }
			|	expr_list COMMA expr				{ $1.expressions.Add($3); $$ = $1; }
			;

assignment	:	secondary ASSIGN expr SEMICOLON		{ $$ = new AssignmentStatemt($1, $3); }
			;

if_stm		:	IF expr THEN stm_list END 						{ $$ = new IfStatement($2, $4, new StatementList()); }
			|	IF expr THEN stm_list ELSE stm_list END			{ $$ = new IfStatement($2, $4, $6); }
			;

loop_stm	:	loop_header LOOP stm_list END		{ $$ = new LoopStatement($1, $3); }
			;

loop_header	:	/* empty */							{ $$ = null; }	/* check */
			|	FOR ID IN expr						{ $$ = new ForHeader(new Identifier($2), $4); }
			|	FOR expr							{ $$ = new ForHeader(null, $2); }
			|	WHILE expr							{ $$ = new WhileHeader($2); }
			;

return_stm	:	RETURN SEMICOLON					{ $$ = new ReturnStatement(); }		/* possible fix recognize new line before END token */
			|	RETURN expr SEMICOLON				{ $$ = new ReturnStatement($2); }
			;

break_stm	:	BREAK SEMICOLON						{ $$ = new BreakStatement(); }
			;

cont_stm	:	CONTINUE SEMICOLON					{ $$ = new ContinueStatement(); }
			;

print_stm	:	PRINT LROUND opt_exprs RROUND SEMICOLON		{ $$ = new PrintStatement($3); }
			;

array_def	:	LSQUARE opt_exprs RSQUARE 			{ $$ = new ArrayDefinition($2); }
			;

map_def		:	LCURLY pair_list RCURLY 			{ $$ = new MapDefinition($2); }
			;

pair_list	:	/* empty */							{ $$ = new ExpressionPairList(); }
			|	pair								{ $$ = new ExpressionPairList($1); }
			|	pair_list COMMA pair				{ $1.pairs.Add($3); $$ = $1; }
			;

pair		:	expr COLON expr 					{ $$ = new PairExpression($1, $3); }
			;

tuple_def	:	LROUND tuple_elist RROUND			{ $$ = new TupleDefinition($2); }
			;

tuple_elist :	tuple_elem							{ $$ = new TupleElementList($1); }
			|	tuple_elist COMMA tuple_elem		{ $1.elements.Add($3); $$ = $1; }
			;

tuple_elem	:	ID IS expr							{ $$ = new TupleElement(new Identifier($1), $3); }
			|	expr								{ $$ = new TupleElement(null, $1); }
			;

indexer		:	LSQUARE expr RSQUARE				{ $$ = new SquaresIndexer($2); }
			|	DOT	ID								{ $$ = new DotIndexer(new Identifier($2), null); }
			|	DOT INTEGER_VALUE					{ $$ = new DotIndexer(null, new IntegerValue($2)); }
			;

func_type	:	FUNC LROUND type_list RROUND COLON type 	{ $$ = new FunctionType($3, $6); }
			;

type_list	:	/* empty */							{ $$ = new TypeList(); }
			|	type								{ $$ = new TypeList($1); }
			|	type_list COMMA type				{ $1.types.Add($3); $$ = $1; }
			;

array_type	:	LSQUARE type RSQUARE 				{ $$ = new ArrayType($2); }
			;

tuple_type	: 	LROUND type_list RROUND				{ $$ = new TupleType($2); }
			;

map_type	:	LCURLY type COLON type RCURLY 		{ $$ = new MapType($2, $4); }
			;
