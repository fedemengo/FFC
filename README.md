# FFC
A compiler for the F language (and it's definition)

## TODOs

* Core features
	* Standard library names can't be reused
	* Fix Type checking in declaration when both are specified
	* Add support to maps
	* Catch exception when using a variable that is not declared

* Minor stuff
	* Add void type keyword to lexical analysis/grammar
	* Create a proper note section, maybe with known bugs?
	* Organize FAST code better in files
	* Move Generate as Generator static method + overload
	* Split Generator class in multiple files ?
	
* Extra features
	* Add := Read ?
	* Load external library from another folder
	* Add warning when declaring variables overriding an existing name
	* Print tuple names when specified
	* Improve Read() run time function
		* Consider Buffer approach
		* More precise "type checking"
		* Consider reading advanced types (arrays, tuples, maps)
		* Allow `\`-escaped characters in string read ?
		* Consider assignment support

* Check if it's done / done correctly
	* Add proper ToString method to FTypes, and maybe even more nodes
	* Check how we deal with types in declaration / indexed access
	* GetValueType in ParameterList / TypeList
	* Type check in recursive functions, error right now for wrong deduction is not very clear, also I don't like setting the type to null, so maybe we should have a RecursionType or something like that - even if null is particularly handy as it gets already recalculated (but we can make recursiontype to be recalculated too!)

* Lexical analysis
	* Think if more `\`-escaped character are needed (currently support `\n`, `\t`, `\\`, `\"`)
	* Think about `\n` instead of `;` for SEMICOLON tokens
	* Add Lexer support for -2^31
	* Handling error token

* Parsing
	* Remove parentheses from (), or determine it's impossible

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
	- [x] Conditional expressions
	- [x] Conditional statements
	- [x] Loop statements
		- [x] While loops
		- [x] For loops
		- [ ] Just loop ?
	- [x] Ellipsis
	- [x] Functions
		- [x] Nested functions
		- [ ] Capturing used variables


- [ ] Debugging and Testing
	- [ ] Lexer error handling support
	- [ ] Parsing error handling support
	- [x] Compilation error handling support
		- [x] Custom Exception type
	- [ ] ~~Runtime error handling support~~

- [ ] Writing a report

## Notes
