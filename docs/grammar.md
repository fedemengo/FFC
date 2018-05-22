################### F-Grammar #########################

elementary  ->  BOOLEAN_VALUE
                INTEGER_VALUE
                REAL_VALUE
                RATIONAL_VALUE
                COMPLEX_VALUE
                STRING_VALUE
                ID

statement   ->  assignment
                *function_call
                if_statement
                loop_statement
                return_statement
                break_statement
                print_statement
                declaration

assignment  ->  ID ASSIGN expression

expression  ->  value
                ID
                expression bin_op expression

bin_op      ->  PLUS
                MINUS
                STAR
                SLASH
                LESS
                LESSEQUAL
                GREATER
                GREATEREQUAL
                EQUAL
                NOTEQUAL
                AND
                OR
                XOR

value       ->  BOOLEAN_VALUE
                INTEGER_VALUE
                REAL_VALUE
                RATIONAL_VALUE
                COMPLEX_VALUE
                STRING_VALUE

declaration ->  ID IS expression
                ID COLON type expression

type        ->  INTEGER
	            COMPLEX
                RATIONAL
                REAL
                STRING
                BOOLEAN
                array_type
                map_type
                func_type

array_type  ->  LSQUARE type RSQUARE

map_type    ->  LCURLY type COLON type RCURLY

map_def     ->  LCURLY RCURLY
                LCURLY pair_list RCURLY

pair        ->  expression COLON expression

pair_list   ->  pair
                pair COMMA pair_list 

func_type   ->  FUNC LROUND type_list RROUND COLON type

func_def    ->  FUNC LROUND param_list RROUND COLON type func_body
                FUNC LROUND param_list RROUND func_body 

func_body   ->  DO statement END
                ARROW statement

type_list   ->  type
                type COMMA type_list

parameter   ->  ID COLON type

param_list  ->  parameter
                parameter COMMA param_list

TODO:   tuples and yaccify

