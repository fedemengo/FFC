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

%using FFC.FAST;
%using FFC.FLexer;
%namespace FFC.FParser
%visibility internal
%partial 

%YYSTYPE TValue

%%


starting    :	dec_list EOF 											{ $$ = $1; }
			;

dec_list	: 	declaration 											{ $$ = new DeclarationStatementList((DeclarationStatement)$1); }
			| 	dec_list declaration									{ ((DeclarationStatementList)$1).statements.Add((DeclarationStatement)$2); $$ = $1; }
			;

declaration	:	identifier opt_type IS expr SEMICOLON					{ $$ = new DeclarationStatement((Identifier)$1, (FType)$2, (FExpression)$4); }
			|	identifier opt_type IS func_def							{ $$ = new DeclarationStatement((Identifier)$1, (FType)$2, (FExpression)$4); }
			;

identifier	:	ID														{ $$ = new Identifier(((TokenValue)$1)[0].ToString()); }
			;

opt_type	:	/* empty */												{ $$ = null; }
			|	COLON type												{ $$ = (FType) $2; }
			;

type		:	INTEGER													{ $$ = new IntegerType(); }
			|	COMPLEX													{ $$ = new ComplexType(); }
			|	RATIONAL												{ $$ = new RationalType(); }
			|	REAL													{ $$ = new RealType(); }
			|	STRING													{ $$ = new StringType(); }
			|	BOOLEAN													{ $$ = new BooleanType(); }
			|	func_type												{ $$ = $1; }
			|	tuple_type												{ $$ = $1; }
			|	array_type												{ $$ = $1; }
			|	map_type												{ $$ = $1; }
			;

expr2		:	expr													{ $$ = $1; }
			|	func_def												{ $$ = $1; }
			;

expr		:	secondary												{ $$ = $1; }
			|	secondary LESS expr										{ $$ = new BinaryOperatorExpression((FSecondary)$1, new LessOperator(), (FExpression)$3); }
			|	secondary LESSEQUAL expr								{ $$ = new BinaryOperatorExpression((FSecondary)$1, new LessEqualOperator(), (FExpression)$3); }	
			|	secondary GREATER expr									{ $$ = new BinaryOperatorExpression((FSecondary)$1, new GreaterOperator(), (FExpression)$3); }
			|	secondary GREATEREQUAL expr								{ $$ = new BinaryOperatorExpression((FSecondary)$1, new GreaterEqualOperator(), (FExpression)$3); }
			|	secondary EQUAL expr									{ $$ = new BinaryOperatorExpression((FSecondary)$1, new EqualOperator(), (FExpression)$3); }
			|	secondary NOTEQUAL expr									{ $$ = new BinaryOperatorExpression((FSecondary)$1, new NotEqualOperator(), (FExpression)$3); }
			|	secondary AND expr										{ $$ = new BinaryOperatorExpression((FSecondary)$1, new AndOperator(), (FExpression)$3); }
			|	secondary OR expr										{ $$ = new BinaryOperatorExpression((FSecondary)$1, new OrOperator(), (FExpression)$3); }
			|	secondary XOR expr										{ $$ = new BinaryOperatorExpression((FSecondary)$1, new XorOperator(), (FExpression)$3); }
			|	secondary PLUS expr										{ $$ = new BinaryOperatorExpression((FSecondary)$1, new PlusOperator(), (FExpression)$3); }
			|	secondary MINUS expr									{ $$ = new BinaryOperatorExpression((FSecondary)$1, new MinusOperator(), (FExpression)$3); }
			|	secondary STAR expr										{ $$ = new BinaryOperatorExpression((FSecondary)$1, new StarOperator(), (FExpression)$3); }
			|	secondary SLASH expr									{ $$ = new BinaryOperatorExpression((FSecondary)$1, new SlashOperator(), (FExpression)$3); }
			|	MINUS secondary %prec NEG								{ $$ = new NegativeExpression((FSecondary)$2); }
			|	secondary ELLIPSIS secondary 							{ $$ = new EllipsisExpression((FSecondary)$1, (FSecondary)$3); }
			;

secondary	:	primary 												{ $$ = $1; }
			|	func_call												{ $$ = $1; }
			|	secondary indexer										{ $$ = new IndexedAccess((FSecondary)$1, (Indexer)$2); }
			;

primary		: 	value													{ $$ = $1; }
			|	cond													{ $$ = $1; }
//			|	func_def												{ $$ = $1; }
			|	array_def												{ $$ = $1; }
			|	map_def													{ $$ = $1; }
			|	tuple_def												{ $$ = $1; }
//			|	LROUND expr RROUND
			;

value		:	BOOLEAN_VALUE											{ $$ = new BooleanValue((bool) ((TokenValue)$1)[0]); }
			|	INTEGER_VALUE											{ $$ = new IntegerValue((int) ((TokenValue)$1)[0]); }	
			|	REAL_VALUE												{ $$ = new RealValue((double) ((TokenValue)$1)[0]); }
			|	RATIONAL_VALUE											{ $$ = new RationalValue((int) ((TokenValue)$1)[0], (int) ((TokenValue)$1)[1]); }
			|	COMPLEX_VALUE											{ $$ = new ComplexValue((double) ((TokenValue)$1)[0], (double) ((TokenValue)$1)[1]); }
			|	STRING_VALUE											{ $$ = new StringValue((string) ((TokenValue)$1)[0]); }
			|	identifier												{ $$ = $1; }
			;

cond		:	IF expr THEN expr2 ELSE expr2 END						{ $$ = new Conditional((FExpression)$2, (FExpression)$4, (FExpression)$6); }
			;

func_def	:	FUNC LROUND opt_params RROUND opt_type func_body		{ $$ = new FunctionDefinition((ParameterList)$3, (FType)$5, (StatementList)$6); }
			;

opt_params	:	/* empty */												{ $$ = new ParameterList(); }
			|	param_list												{ $$ = $1; }
			;

param_list	:	param 													{ $$ = new ParameterList((Parameter)$1); }
			| 	param_list COMMA param									{ ((ParameterList)$1).parameters.Add((Parameter)$3); $$ = $1; }
			;

param		:	identifier COLON type									{ $$ = new Parameter((Identifier)$1, (FType)$3); }
			;

func_body	:	DO stm_list END											{ $$ = $2; }
			|	ARROW LROUND expr2 RROUND								{ $$ = new StatementList(new ExpressionStatement((FExpression)$3)); }
			;

stm_list	:	statement												{ $$ = new StatementList((FStatement)$1); }
			|	stm_list statement										{ ((StatementList)$1).statements.Add((FStatement)$2); $$ = $1; }
			;

ni_stm		:	func_call SEMICOLON										{ $$ = $1; }
			|	assignment												{ $$ = $1; }
			|	declaration												{ $$ = $1; }
			|	loop_stm												{ $$ = $1; }
			|	return_stm												{ $$ = $1; }
			|	break_stm												{ $$ = $1; }
			|	cont_stm												{ $$ = $1; }
			|	print_stm												{ $$ = $1; }
			;

statement	: 	if_stm													{ $$ = $1; }
			|	ni_stm													{ $$ = $1; }
			;

func_call	:	secondary LROUND opt_exprs RROUND						{ $$ = new FunctionCall((FSecondary)$1, (ExpressionList)$3); }
			;

opt_exprs	:	/* empty */												{ $$ = new ExpressionList(); }
			|	expr_list												{ $$ = $1; }
			;

expr_list	:	expr2													{ $$ = new ExpressionList((FExpression)$1); }
			|	expr_list COMMA expr2									{ ((ExpressionList)$1).expressions.Add((FExpression)$3); $$ = $1; }
			;

assignment	:	secondary ASSIGN expr2 SEMICOLON						{ $$ = new AssignmentStatemt((FSecondary)$1, (FExpression)$3); }
			;

if_stm		: 	IF expr THEN stm_list e_if_list opt_else				{ $$ = new IfStatement((FExpression)$2, (StatementList)$4, (ElseIfList)$5, (StatementList)$6); }
			|	IF expr THEN stm_list opt_else							{ $$ = new IfStatement((FExpression)$2, (StatementList)$4, new ElseIfList(), (StatementList)$5); }
			;

e_if_list	:	ELSE IF expr THEN stm_list								{ $$ = new ElseIfList(new ElseIfStatement((FExpression) $3, (StatementList) $5)); }
			|	e_if_list ELSE IF expr THEN stm_list					{ ((ElseIfList)($1)).Add(new ElseIfStatement((FExpression) $4, (StatementList) $6)); $$ = $1; }
			;
		
opt_else	:	END														{ $$ = new StatementList(); }
			|	ELSE ni_stm_list END									{ $$ = $2; }
			;

ni_stm_list :	ni_stm													{ $$ = new StatementList((FStatement)$1); }
			|	ni_stm_list statement									{ ((StatementList)$1).Add((FStatement)$2); $$ = $1; }
			;

loop_stm	:	loop_header LOOP stm_list END							{ $$ = new LoopStatement((FLoopHeader)$1, (StatementList)$3); }
			;

loop_header	:	/* empty */												{ $$ = null; }	/* check */
			|	FOR identifier IN expr									{ $$ = new ForHeader((Identifier)$2, (FExpression)$4); }
			|	FOR expr												{ $$ = new ForHeader(null, (FExpression)$2); }
			|	WHILE expr												{ $$ = new WhileHeader((FExpression)$2); }
			;

return_stm	:	RETURN SEMICOLON										{ $$ = new ReturnStatement(); }		/* possible fix recognize new line before END token */
			|	RETURN expr2 SEMICOLON									{ $$ = new ReturnStatement((FExpression)$2); }
			;

break_stm	:	BREAK SEMICOLON											{ $$ = new BreakStatement(); }
			;

cont_stm	:	CONTINUE SEMICOLON										{ $$ = new ContinueStatement(); }
			;

print_stm	:	PRINT LROUND opt_exprs RROUND SEMICOLON					{ $$ = new PrintStatement((ExpressionList)$3); }
			;

array_def	:	LSQUARE opt_exprs RSQUARE 								{ $$ = new ArrayDefinition((ExpressionList)$2); }
			;

map_def		:	LCURLY pair_list RCURLY 								{ $$ = new MapDefinition((ExpressionPairList)$2); }
			;

pair_list	:	/* empty */												{ $$ = new ExpressionPairList(); }
			|	pair													{ $$ = new ExpressionPairList((ExpressionPair)$1); }
			|	pair_list COMMA pair									{ ((ExpressionPairList)$1).pairs.Add((ExpressionPair)$3); $$ = $1; }
			;

pair		:	expr2 COLON expr2										{ $$ = new ExpressionPair((FExpression)$1, (FExpression)$3); }
			;

tuple_def	:	LROUND tuple_elist RROUND								{ $$ = new TupleDefinition((TupleElementList)$2); }
			;

tuple_elist :	tuple_elem												{ $$ = new TupleElementList((TupleElement)$1); }
			|	tuple_elist COMMA tuple_elem							{ ((TupleElementList)$1).elements.Add((TupleElement)$3); $$ = $1; }
			;

tuple_elem	:	identifier IS expr2										{ $$ = new TupleElement((Identifier)$1, (FExpression)$3); }
			|	expr2													{ $$ = new TupleElement(null, (FExpression)$1); }
			;

indexer		:	LSQUARE expr2 RSQUARE									{ $$ = new SquaresIndexer((FExpression)$2); }
			|	DOT	identifier											{ $$ = new DotIndexer((Identifier)$2, null);}
			|	DOT INTEGER_VALUE										{ $$ = new DotIndexer(null, new IntegerValue((int)((TokenValue)$2)[0])); }
			;

func_type	:	FUNC LROUND type_list RROUND COLON type 				{ $$ = new FunctionType((TypeList)$3, (FType)$6); }
			;

type_list	:	/* empty */												{ $$ = new TypeList(); }
			|	type													{ $$ = new TypeList((FType)$1); }
			|	type_list COMMA type									{ ((TypeList)$1).types.Add((FType)$3); $$ = $1; }
			;

array_type	:	LSQUARE type RSQUARE 									{ $$ = new ArrayType((FType)$2); }
			;

tuple_type	: 	LROUND type_list RROUND									{ $$ = new TupleType((TypeList)$2); }
			;

map_type	:	LCURLY type COLON type RCURLY 							{ $$ = new MapType((FType)$2, (FType)$4); }
			;
