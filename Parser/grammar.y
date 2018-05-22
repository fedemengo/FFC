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
%token WHILE FOR IN LOOP BREAK
%token LROUND RROUND LSQUARE RSQUARE LCURLY RCURLY

%token EOF

%start starting

%%

starting    :   stm_list EOF

stm_list    :   statement SEMICOLON
            |   statement SEMICOLON stm_list
            ;

statement   :   assignment
            |   expression
            |   if_statement
            |   loop_statement
            |   return_statement
            |   break_statement
            |   print_statement
            |   declaration
			;

assignment  :   ID ASSIGN expression ;

func_call   :   expression LROUND expr_list RROUND ;

if_statement:   IF expression THEN stm_list END
            |   IF expression THEN stm_list ELSE stm_list END
            ;

loop_statement: loop_header LOOP stm_list END
            ; 
        
loop_header : FOR ID IN expr_list 
            | FOR expr_list
            | WHILE expression
            ;

return_statement : RETURN expression ;

break_statement : BREAK ;

print_statement : PRINT LROUND expr_list RROUND ;

expression  :  value
			|  ID
            |  neg_expr
			|  expression bin_op expression
            |  func_call
            |  func_def
            |  map_def
            |  LROUND expression RROUND
			;

neg_expr    : MINUS expression ;

expr_list   : expression
            | expression COMMA expr_list
            ;

bin_op      :   STAR
            |   SLASH
            |   PLUS
            |   MINUS
            |   LESS
			|   LESSEQUAL
			|   GREATER
			|   GREATEREQUAL
			|   EQUAL
			|   NOTEQUAL
			|   AND
			|   OR
			|   XOR
			;

value       :   BOOLEAN_VALUE
			|   INTEGER_VALUE
			|   REAL_VALUE
			|   RATIONAL_VALUE
			|   COMPLEX_VALUE
			|   STRING_VALUE
			;

declaration :   ID IS expression
			|   ID COLON type expression
			;

type        :   INTEGER
	        |   COMPLEX
			|   RATIONAL
			|   REAL
			|   STRING
			|   BOOLEAN
			|   array_type
			|   map_type
			|   func_type
			;

array_type  :  LSQUARE type RSQUARE ;

map_type    :  LCURLY type COLON type RCURLY ;

map_def     :  LCURLY RCURLY
			|  LCURLY pair_list RCURLY
			;

pair        :  expression COLON expression ;

pair_list   :  pair
			|  pair COMMA pair_list 
			;

func_type   :  FUNC LROUND type_list RROUND COLON type ;

func_def    :  FUNC LROUND param_list RROUND COLON type func_body
			|  FUNC LROUND param_list RROUND func_body
			;

func_body   :  DO stm_list END
			|  ARROW expression 
			;

type_list   :  type
			|  type COMMA type_list
			;

parameter   :  ID COLON type ;

param_list  :  parameter
			|  parameter COMMA param_list
			;

%%
