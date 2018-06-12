%tokentype ETokens

%token ID BOOLEAN_VALUE INTEGER_VALUE REAL_VALUE RATIONAL_VALUE COMPLEX_VALUE STRING_VALUE
%token DOT COMMA COLON SEMICOLON
%token INTEGER COMPLEX RATIONAL REAL STRING BOOLEAN
%token ASSIGN ARROW
%token FUNC DO RETURN PRINT
%token IS IF THEN ELSE END
%token WHILE FOR IN LOOP BREAK CONTINUE
%token LROUND RROUND LSQUARE RSQUARE LCURLY RCURLY
%token ELLIPSIS
%token EOF

%left NOT
%left OR XOR
%left AND
%left LESS LESSEQUAL GREATER GREATEREQUAL EQUAL NOTEQUAL
%left PLUS MINUS
%left STAR SLASH MODULO

%start starting

%using FFC.FAST;
%using FFC.FLexer;
%namespace FFC.FParser
%visibility internal
%partial 

%YYSTYPE TValue

%%

starting    :	dec_list EOF 											{ $$ = $1; }
			;

dec_list	: 	declaration 											{ $$ = new DeclarationStatementList((DeclarationStatement)$1, new TextSpan($1.Span)); }
			| 	dec_list declaration									{ ((DeclarationStatementList)$1).Add((DeclarationStatement)$2); $1.Span = $1.Span.MergeTo($2.Span); $$ = $1; }
			;

declaration	:	identifier opt_type IS math_expr SEMICOLON				{ $$ = new DeclarationStatement((Identifier)$1, (FType)$2, (FExpression)$4, new TextSpan($1.Span, $5.Span)); }
			|	identifier opt_type IS func_def							{ $$ = new DeclarationStatement((Identifier)$1, (FType)$2, (FExpression)$4, new TextSpan($1.Span, $4.Span)); }
			;

identifier	:	ID														{ $$ = new Identifier(((TokenValue)$1)[0].ToString(), new TextSpan($1.Span)); }
			;

opt_type	:	/* empty */												{ $$ = null; }
			|	COLON type												{ $$ = $2; }
			;

type		:	INTEGER													{ $$ = new IntegerType(new TextSpan($1.Span)); }
			|	COMPLEX													{ $$ = new ComplexType(new TextSpan($1.Span)); }
			|	RATIONAL												{ $$ = new RationalType(new TextSpan($1.Span)); }
			|	REAL													{ $$ = new RealType(new TextSpan($1.Span)); }
			|	STRING													{ $$ = new StringType(new TextSpan($1.Span)); }
			|	BOOLEAN													{ $$ = new BooleanType(new TextSpan($1.Span)); }
			|	func_type												{ $$ = $1; }
			|	tuple_type												{ $$ = $1; }
			|	array_type												{ $$ = $1; }
			|	map_type												{ $$ = $1; }
			;

expr		:	math_expr												{ $$ = $1; }
			|	func_def												{ $$ = $1; }
			;

math_expr	:	secondary												{ $$ = $1; }
			|	NOT math_expr											{ $$ = new NotExpression((FExpression)$2, new TextSpan($2.Span)); }
			|	math_expr OR math_expr									{ $$ = new BinaryOperatorExpression((FExpression)$1, new OrOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr XOR math_expr									{ $$ = new BinaryOperatorExpression((FExpression)$1, new XorOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr AND math_expr									{ $$ = new BinaryOperatorExpression((FExpression)$1, new AndOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr LESS math_expr								{ $$ = new BinaryOperatorExpression((FExpression)$1, new LessOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr LESSEQUAL math_expr							{ $$ = new BinaryOperatorExpression((FExpression)$1, new LessEqualOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }	
			|	math_expr GREATER math_expr								{ $$ = new BinaryOperatorExpression((FExpression)$1, new GreaterOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr GREATEREQUAL math_expr						{ $$ = new BinaryOperatorExpression((FExpression)$1, new GreaterEqualOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr NOTEQUAL math_expr							{ $$ = new BinaryOperatorExpression((FExpression)$1, new NotEqualOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr PLUS math_expr								{ $$ = new BinaryOperatorExpression((FExpression)$1, new PlusOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr EQUAL math_expr								{ $$ = new BinaryOperatorExpression((FExpression)$1, new EqualOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr MINUS math_expr								{ $$ = new BinaryOperatorExpression((FExpression)$1, new MinusOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr STAR math_expr								{ $$ = new BinaryOperatorExpression((FExpression)$1, new StarOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr SLASH math_expr								{ $$ = new BinaryOperatorExpression((FExpression)$1, new SlashOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	math_expr MODULO math_expr								{ $$ = new BinaryOperatorExpression((FExpression)$1, new ModuloOperator($2.Span), (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	MINUS secondary %prec NEG								{ $$ = new NegativeExpression((FSecondary)$2, new TextSpan($1.Span, $2.Span)); }
			|	secondary ELLIPSIS secondary 							{ $$ = new EllipsisExpression((FSecondary)$1, (FSecondary)$3, new TextSpan($1.Span, $3.Span)); }
			;

secondary	:	primary 												{ $$ = $1; }
			|	func_call												{ $$ = $1; }
			|	secondary indexer										{ $$ = new IndexedAccess((FSecondary)$1, (Indexer)$2, new TextSpan($1.Span, $2.Span)); }
			;

primary		: 	value													{ $$ = $1; }
			|	cond													{ $$ = $1; }
			|	array_def												{ $$ = $1; }
			|	map_def													{ $$ = $1; }
			|	tuple_def												{ $$ = $1; }
//			|	LROUND math_expr RROUND
			;

value		:	BOOLEAN_VALUE											{ $$ = new BooleanValue((bool) ((TokenValue)$1)[0], new TextSpan($1.Span)); }
			|	INTEGER_VALUE											{ $$ = new IntegerValue((int) ((TokenValue)$1)[0], new TextSpan($1.Span)); }	
			|	REAL_VALUE												{ $$ = new RealValue((double) ((TokenValue)$1)[0], new TextSpan($1.Span)); }
			|	RATIONAL_VALUE											{ $$ = new RationalValue((int) ((TokenValue)$1)[0], (int) ((TokenValue)$1)[1], new TextSpan($1.Span)); }
			|	COMPLEX_VALUE											{ $$ = new ComplexValue((double) ((TokenValue)$1)[0], (double) ((TokenValue)$1)[1], new TextSpan($1.Span)); }
			|	STRING_VALUE											{ $$ = new StringValue((string) ((TokenValue)$1)[0], new TextSpan($1.Span)); }
			|	identifier												{ $$ = $1; }
			;

cond		:	IF math_expr THEN expr ELSE expr END					{ $$ = new Conditional((FExpression)$2, (FExpression)$4, (FExpression)$6, new TextSpan($1.Span, $7.Span)); }
			;

func_def	:	FUNC LROUND opt_params RROUND opt_type func_body		{ $$ = new FunctionDefinition((ParameterList)$3, (FType)$5, (StatementList)$6, new TextSpan($1.Span, $6.Span)); }
			;

opt_params	:	/* empty */												{ $$ = new ParameterList(); }
			|	param_list												{ $$ = $1; }
			;

param_list	:	param 													{ $$ = new ParameterList((Parameter)$1, new TextSpan($1.Span)); }
			| 	param_list COMMA param									{ ((ParameterList)$1).parameters.Add((Parameter)$3); $1.Span = $1.Span.MergeTo($3.Span); $$ = $1; }
			;

param		:	identifier COLON type									{ $$ = new Parameter((Identifier)$1, (FType)$3, new TextSpan($1.Span, $3.Span)); }
			;

func_body	:	DO stm_list END											{ $$ = $2; }
			|	ARROW LROUND expr RROUND								{ $$ = new StatementList(new ExpressionStatement((FExpression)$3, new TextSpan($3.Span)), new TextSpan($1.Span, $4.Span)); }
			;

stm_list	:	statement												{ $$ = new StatementList((FStatement)$1, new TextSpan($1.Span)); }
			|	stm_list statement										{ ((StatementList)$1).statements.Add((FStatement)$2); $1.Span = $1.Span.MergeTo($2.Span); $$ = $1; }
			;

nif_stm		:	func_call SEMICOLON										{ $$ = $1; }
			|	assignment												{ $$ = $1; }
			|	declaration												{ $$ = $1; }
			|	loop_stm												{ $$ = $1; }
			|	return_stm												{ $$ = $1; }
			|	break_stm												{ $$ = $1; }
			|	cont_stm												{ $$ = $1; }
			|	print_stm												{ $$ = $1; }
			;

statement	: 	if_stm													{ $$ = $1; }
			|	nif_stm													{ $$ = $1; }
			;

func_call	:	secondary LROUND opt_exprs RROUND						{ $$ = new FunctionCall((FSecondary)$1, (ExpressionList)$3, new TextSpan($1.Span, $4.Span)); }
			;

opt_exprs	:	/* empty */												{ $$ = new ExpressionList(); }
			|	expr_list												{ $$ = $1; }
			;

expr_list	:	expr													{ $$ = new ExpressionList((FExpression)$1, new TextSpan($1.Span)); }
			|	expr_list COMMA expr									{ ((ExpressionList)$1).expressions.Add((FExpression)$3); $1.Span = $1.Span.MergeTo($3.Span); $$ = $1; }
			;

assignment	:	secondary ASSIGN expr SEMICOLON							{ $$ = new AssignmentStatemt((FSecondary)$1, (FExpression)$3, new TextSpan($1.Span, $4.Span)); }
			;

if_stm		: 	IF math_expr THEN stm_list e_if_list opt_else			{ $$ = new IfStatement((FExpression)$2, (StatementList)$4, (ElseIfList)$5, (StatementList)$6, new TextSpan($1.Span, $6.Span)); } //even if last is optional, it's always at least END so it has a TextSpan value 
			|	IF math_expr THEN stm_list opt_else						{ $$ = new IfStatement((FExpression)$2, (StatementList)$4, new ElseIfList(), (StatementList)$5, new TextSpan($1.Span, $5.Span)); }
			;

e_if_list	:	ELSE IF math_expr THEN stm_list							{ $$ = new ElseIfList(new ElseIfStatement((FExpression) $3, (StatementList) $5, new TextSpan($1.Span, $5.Span)), new TextSpan($1.Span, $5.Span)); }
			|	e_if_list ELSE IF math_expr THEN stm_list				{ ((ElseIfList)($1)).Add(new ElseIfStatement((FExpression) $4, (StatementList) $6, new TextSpan($2.Span, $6.Span))); $1.Span = $1.Span.MergeTo($6.Span); $$ = $1; }
			;
		
opt_else	:	END														{ $$ = new StatementList(new TextSpan($1.Span)); }
			|	ELSE nif_stmlist END									{ $2.Span = new TextSpan($1.Span, $3.Span); $$ = $2; }
			;

nif_stmlist :	nif_stm													{ $$ = new StatementList((FStatement)$1, $1.Span); }
			|	nif_stmlist statement									{ ((StatementList)$1).Add((FStatement)$2); $1.Span = $1.Span.MergeTo($2.Span); $$ = $1; }
			;

loop_stm	:	loop_header LOOP stm_list END							{ $$ = new LoopStatement((FLoopHeader)$1, (StatementList)$3, new TextSpan($1.Span, $4.Span)); }
			;

loop_header	:	/* empty */												{ $$ = null; }	/* check */
			|	FOR identifier IN math_expr								{ $$ = new ForHeader((Identifier)$2, (FExpression)$4, new TextSpan($1.Span, $4.Span)); }
			|	FOR math_expr											{ $$ = new ForHeader(null, (FExpression)$2, new TextSpan($1.Span, $2.Span)); }
			|	WHILE math_expr											{ $$ = new WhileHeader((FExpression)$2,new TextSpan($1.Span, $2.Span)); }
			;

return_stm	:	RETURN SEMICOLON										{ $$ = new ReturnStatement(new TextSpan($1.Span)); }		/* possible fix recognize new line before END token */
			|	RETURN expr SEMICOLON									{ $$ = new ReturnStatement((FExpression)$2, new TextSpan($1.Span)); }
			;

break_stm	:	BREAK SEMICOLON											{ $$ = new BreakStatement(new TextSpan($1.Span)); }
			;

cont_stm	:	CONTINUE SEMICOLON										{ $$ = new ContinueStatement(new TextSpan($1.Span)); }
			;

print_stm	:	PRINT LROUND opt_exprs RROUND SEMICOLON					{ $$ = new PrintStatement((ExpressionList)$3, new TextSpan($1.Span, $5.Span)); }
			;

array_def	:	LSQUARE opt_exprs RSQUARE 								{ $$ = new ArrayDefinition((ExpressionList)$2, new TextSpan($1.Span, $3.Span)); }
			;

map_def		:	LCURLY pair_list RCURLY 								{ $$ = new MapDefinition((ExpressionPairList)$2, new TextSpan($1.Span, $3.Span)); }
			;

pair_list	:	/* empty */												{ $$ = new ExpressionPairList(); }
			|	pair													{ $$ = new ExpressionPairList((ExpressionPair)$1, new TextSpan($1.Span)); }
			|	pair_list COMMA pair									{ ((ExpressionPairList)$1).pairs.Add((ExpressionPair)$3); $1.Span = $1.Span.MergeTo($3.Span); $$ = $1; }
			;

pair		:	expr COLON expr											{ $$ = new ExpressionPair((FExpression)$1, (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			;

tuple_def	:	LROUND tuple_elist RROUND								{ $$ = new TupleDefinition((TupleElementList)$2, new TextSpan($1.Span, $3.Span)); }
			;

tuple_elist :	tuple_elem												{ $$ = new TupleElementList((TupleElement)$1, new TextSpan($1.Span)); }
			|	tuple_elist COMMA tuple_elem							{ ((TupleElementList)$1).elements.Add((TupleElement)$3); $1.Span = $1.Span.MergeTo($3.Span); $$ = $1; }
			;

tuple_elem	:	identifier IS expr										{ $$ = new TupleElement((Identifier)$1, (FExpression)$3, new TextSpan($1.Span, $3.Span)); }
			|	expr													{ $$ = new TupleElement(null, (FExpression)$1, new TextSpan($1.Span)); }
			;

indexer		:	LSQUARE expr RSQUARE									{ $$ = new SquaresIndexer((FExpression)$2, new TextSpan($1.Span, $3.Span)); }
			|	DOT	identifier											{ $$ = new DotIndexer((Identifier)$2, null, new TextSpan($1.Span, $2.Span));}
			|	DOT INTEGER_VALUE										{ $$ = new DotIndexer(null, new IntegerValue((int)((TokenValue)$2)[0], $2.Span), new TextSpan($1.Span, $2.Span)); }
			;

func_type	:	FUNC LROUND type_list RROUND COLON type 				{ $$ = new FunctionType((TypeList)$3, (FType)$6, new TextSpan($1.Span, $6.Span)); }
			;

type_list	:	/* empty */												{ $$ = new TypeList(); }
			|	type													{ $$ = new TypeList((FType)$1, new TextSpan($1.Span)); }
			|	type_list COMMA type									{ ((TypeList)$1).types.Add((FType)$3); $1.Span = $1.Span.MergeTo($3.Span); $$ = $1; }
			;

array_type	:	LSQUARE type RSQUARE 									{ $$ = new ArrayType((FType)$2, new TextSpan($1.Span, $3.Span)); }
			;

tuple_type	: 	LROUND type_list RROUND									{ $$ = new TupleType((TypeList)$2, new TextSpan($1.Span, $3.Span)); }
			;

map_type	:	LCURLY type COLON type RCURLY 							{ $$ = new MapType((FType)$2, (FType)$4, new TextSpan($1.Span, $5.Span)); }
			;
