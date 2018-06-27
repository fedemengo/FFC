# FFC
A compiler for the F language (and it's definition)

## TODOs

* Update note section
* Check why current func_declaration.f not working (might have to do with parameters)
* Fix Type checking in declaration when both are specified
* Add := FunctionDefinition in grammar
* Add proper ToString method to FTypes, and maybe even more nodes
* Support to void functions ?
* GetValueType in ParameterList / TypeList
* Organize FAST code better in files
* Load external library from another folder
* Catch exception in wrong.f ?
* Improve Read() run time function
	* Consider Buffer approach
	* More precise "type checking"
	* Consider reading advanced types (arrays, tuples, maps)
	* Allow `\`-escaped characters in string read ?
	* Consider assignment support
* Improve for / foreach behavior, so that values can be modified
* Catch exception when using a variable that is not declared
* Add warning when declaring variables overriding an existing name

* Lexical analysis
	* Think if more `\`-escaped character are needed (currently support `\n`, `\t`, `\\`, `\"`)
	* Think about `\n` instead of `;` for SEMICOLON tokens
	* Add Lexer support for -2^31
	* Handling error token

* Parsing
	* Nothing - except if we decide to think that we should READ assignments or other stuff too

* Code generation
	* Modify generate method to always carry the needed information (loop label, ..)
	* Check how we deal with types in declaration / indexed access

## Progress

- [x] Lexical analysis
	
- [x] Parsing
	- [x] Grammar	
	- [x] Semantic actions
	- [x] AST structure

- [x] Mappings (main constructs, nested functions)
	- Refer to code generation progress

- [ ] Code generation (while creating mappings)
	- [x] Expressions
	- [x] Print
	- [x] Read
	- [x] Symbols lookup
		- [x] Nested scopes
	- [x] Declarations
	- [x] Assignments
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
	- [x] Tuples
		- [ ] Define type for tuple (useful in assignment) 
		- [ ] Consider printing identifiers when present  
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
