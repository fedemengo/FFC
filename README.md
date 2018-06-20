# FFC
A compiler for the F language (and it's definition)

## TODOs

* Update note section
* Load external library from another folder
* Shall we accept INT i INT as complex?
	* sfilippo(s)'s 0.02$ : "we should, but also in Lexer to keep it consistent"
* Improve Read() run time function
	* consider Buffer approach
	* more precise "type checking"
	* consider reading advanced types (arrays, tuples, maps)
	* Allow `\`-escaped characters in string read ?
* Add warning when declaring variables overriding an existing name

* Lexical analysis
	* Think if more `\`-escaped character are needed (currently support `\n`, `\t`, `\\`, `\"`)
	* Think about `\n` instead of `;` for SEMICOLON tokens
	* Add Lexer support for -2^31
	* Handling error token

* Parsing
	* Nothing - except if we decide to think that we should READ assignements or other stuff too

* Code generation
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
		- [x] Empty arrays
		- [x] Concatenation
		- [x] Indexed access
		- [x] Iterator
	- [x] Strings
		* Same operators as arrays ?
	- [ ] Maps
	- [ ] Tuples
	- [x] Conditional expressions
	- [x] Conditional statements
	- [x] Loop statements
		- [x] While loops
		- [x] For loops
		- [ ] Just loop ?
	- [x] Ellipsis
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

* No main function, the entry point is specified at compile time `compile source.f entry_point`
