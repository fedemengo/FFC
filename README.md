# FFC
A compiler for the F language (and it's definition)

## TODOs
* Check this TODO list
* Action on the tree 
	* Code generation (e.g. If is smth in stack and some conditional jump) (recursively within different statement)
	* Reorganize tree (optimization)
* Mappings between current and final code
* Dynamically load external library 

* Think if more `\` escaped character are needed (supported `\n`, `\t`, `\\`, `\"`)
* Think about `\n` instead of `;` for SEMICOLON tokens
* Add Lexer support for -2^31

* Handling error token

## Progress

- [x] Lexical analysis
	
- [x] Parsing
	- [x] Grammar	
	- [x] Semantic actions
	- [x] AST structure

- [ ] Mappings (main constructs, nested functions)
	- Refer to code generation progress

- [ ] Code generation (while creating mappings)
	- [x] Expressions
	- [x] Output
	- [ ] Input
	- [ ] Symbols lookup
		- [ ] Nested scopes
	- [x] Numeric types
	- [x] Arrays
		- [ ] Empty arrays
		- [x] Concatenation
	- [ ] Strings
	- [ ] Maps
	- [ ] Tuples
	- [ ] Conditional expressions
	- [ ] Conditional statements
	- [ ] Loop statements
	- [ ] Ellipsis
	- [ ] Functions
		- [ ] Nested functions

- [ ] Debugging and Testing
	- [ ] Lexer error handling support
	- [ ] Parsing error handling support
	- [ ] Compilation error handling support
	- [ ] ~~Runtime error handling support~~

- [ ] Writing a report

## Notes

* **dotnet core** CIL generation https://github.com/dotnet/coreclr
