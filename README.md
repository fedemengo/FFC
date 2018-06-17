# FFC
A compiler for the F language (and it's definition)

## TODOs

* Update note section
* Load external library from another folder
* Investigate complex number read behaviour
	* Shall we accept INT i INT as complex?
* Improve Read() run time function
	* consider Buffer approach
	* more precise "type checking"
	* consider reading advanced types (arrays, tuples, maps)
* Add warning when declaring variables overriding an existing name

* Lexical analysis
	* Think if more `\`-escaped character are needed (currently support `\n`, `\t`, `\\`, `\"`)
	* Think about `\n` instead of `;` for SEMICOLON tokens
	* Add Lexer support for -2^31
	* Handling error token

* Parsing
	* Nothing - except if we decide to think that we should READ assignements or other stuff too

* Code generation
	* Fix SymbolTable class so that (type casting) is not needed
	* Check how we deal with types in declaration / indexed access

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
	- [x] Print
	- [x] Read
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
		- [x] Indexed access
		- [ ] Type checking in indexed access !!!
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
