# FFC
A compiler for the F language (and it's definition)

## TODOs

* Update note section
* Load external library from another folder

* Lexical analysis
	* Think if more `\`-escaped character are needed (currently support `\n`, `\t`, `\\`, `\"`)
	* Think about `\n` instead of `;` for SEMICOLON tokens
	* Add Lexer support for -2^31
	* Handling error token
	* Add `read` keyword

* Parsing
	* Add `READ` token to declaration statements rules
	* Maybe even to assignements ?

* Code generation
	* Fix SymbolTable class so that (type casting) is not needed

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
	- [x] Symbols lookup
		- [x] Nested scopes
	- [x] Declarations
	- [x] Assignements
	- [x] Numeric types
		- [x] Operators
	- [ ] Standard library functions
	- [x] Arrays
		- [ ] Empty arrays
		- [x] Concatenation
		- [ ] Indexed access
	- [x] Strings
		* Same operators as arrays ?
	- [ ] Maps
	- [ ] Tuples
	- [x] Conditional expressions
	- [x] Conditional statements
	- [ ] Loop statements
		- [x] While loops
		- [ ] For loops
	- [ ] Ellipsis
	- [ ] Functions
		- [ ] Nested functions

- [ ] Debugging and Testing
	- [ ] Lexer error handling support
	- [ ] Parsing error handling support
	- [x] Compilation error handling support
		- [ ] Custom Exception type
	- [ ] ~~Runtime error handling support~~

- [ ] Writing a report

## Notes

* **dotnet core** CIL generation https://github.com/dotnet/coreclr
