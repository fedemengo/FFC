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
            |   if_stm
            |   loop_stm
            |   return_stm
            |   break_stm
            |   print_stm
            |   declaration
			;

assignment  :   ID ASSIGN expression ;

func_call   :   expression LROUND expr_list RROUND ;	/* 2 conflict (1 + lambda) */

if_stm		:   IF expression THEN stm_list END
            |   IF expression THEN stm_list ELSE stm_list END
            ;

loop_stm	: 	loop_header LOOP stm_list END
            ; 
        
loop_header : 	FOR ID IN expr_list 
            | 	FOR expr_list
            | 	WHILE expression
            ;

return_stm	: 	RETURN expression ;

break_stm	: 	BREAK ;

print_stm	: 	PRINT LROUND expr_list RROUND ;

expression  :   expression cmp_op expression
			|   expression logic_op expression
			|   expression add_op expression
			|   expression mul_op expression
            |   neg_expr
            |   func_def
			|	func_call
            |   map_def
            |   LROUND expression RROUND
			|   ID
            |   value
			;

neg_expr    : 	MINUS expression ;

expr_list   : 	expression
            | 	expression COMMA expr_list
            ;

cmp_op		:	LESS
			|   LESSEQUAL
			|   GREATER
			|   GREATEREQUAL
			|   EQUAL
			|   NOTEQUAL
			;

logic_op	:	AND
			|   OR
			|   XOR
			;

add_op		:	PLUS
            |   MINUS
			;

mul_op      :   STAR
            |   SLASH
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

array_type  :  	LSQUARE type RSQUARE ;

map_type    :  	LCURLY type COLON type RCURLY ;

map_def     :  	LCURLY RCURLY
			|  	LCURLY pair_list RCURLY
			;

pair        :  	expression COLON expression ;

pair_list   :  	pair
			|  	pair COMMA pair_list 
			;

func_type   :  	FUNC LROUND type_list RROUND COLON type ;

func_def    :  	FUNC LROUND param_list RROUND COLON type func_body
			|  	FUNC LROUND param_list RROUND func_body
			;

func_body   :  	DO stm_list END
			|  	ARROW expression	/* 1 conflict with func_call */
			;

type_list   :  	type
			|  	type COMMA type_list
			;

parameter   :  	ID COLON type ;

param_list  :  	parameter
			|  	parameter COMMA param_list
			;

%%
