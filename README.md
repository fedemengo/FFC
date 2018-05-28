# F
A compiler for the F language (and it's definition)

## TODOs
* tuples vs expression within parenthesis (no more tuples with just one unnamed value)

* Design structure of statement (with feature to print AST)
* Action on the tree 
	* Code generation (e.g. If is smth in stack and some conditional jump) (recursively within different statement)
	* ?
	* Reorganize tree (optimization)
* Mappings between current and final code
* Generate code for simple program (e.g "hello world" and look into it with **ildasm.exe**)
* Dynamically load external library 

## SCHEDULE

* W1 - AST (structure), semantic actions

* W2 - Mappings (for main constructs and for nested functions)

* W3 - Code generation (while creating mappings)

* W4 - Debugging and Testing

* W5 - Writing a report

## Progress

- Lexer
    - [ ] Bug detection

- Parser
    - [ ] Operator precedence
    - [ ] Tuples
