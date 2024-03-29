# FFC
A compiler for the F language (and it's definition)

## TODOs

* Core features
	* Standard library
		* Names can't be reused
	* Add support to maps [!]
	* Add void type keyword in both Lexer and Parser

* Extra features
	* Add := Read (?)
	* Add warning when declaring variables overriding an existing name
	* Print tuple names when specified
	* Improve Read() RunTime function
		* Consider Buffer approach
		* More flexible "type checking" (trim whitespaces and the like)
		* Consider reading advanced types (arrays, tuples, maps)
		* Allow `\`-escaped characters in string read ?
	* Load external library from another folder

* Code organization
	* Split Generator class in multiple files
	* Organize FAST code better in files
	* Move Generate as Generator static method + overload
	
* Check if it's done correctly
	* GetTarget / Comparisons of non-NumericTypes
	* Check how we deal with types in declaration / indexed access
	* GetValueType in ParameterList / TypeList
	* Type check in recursive functions, error right now for wrong deduction is not very clear, also I don't like setting the type to null, so maybe we should have a RecursionType or something like that - even if null is particularly handy as it gets already recalculated (but we can make recursiontype to be recalculated too!)
	* Check behaviours of iterators/ellipsis to see if they are well integrated with last features (functions and capturing in particular)
	* Loops without header (?)

* Lexical analysis
	* Think if more `\`-escaped character are needed (currently support `\n`, `\t`, `\\`, `\"`)
	* Think about `\n` instead of `;` for SEMICOLON tokens
	* Add Lexer support for -2^31
	* Handling error token

* Parsing
	* Remove parentheses from `func => ()`, or determine it's impossible

## Progress

- [x] Lexical analysis
	
- [x] Parsing
	- [x] Grammar	
	- [x] Semantic actions
	- [x] AST structure

- [x] Mappings (main constructs, nested functions)
	- Refer to code generation progress

- [x] Code generation (while creating mappings)
	- [x] Expressions
	- [x] Print
	- [x] Read
	- [x] Symbols lookup
		- [x] Nested scopes
	- [x] Declarations
	- [x] Assignments
	- [x] Numeric types
		- [x] Operators
	- [x] Standard library functions
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
		- [x] Just loop
	- [x] Ellipsis
	- [x] Functions
		- [x] Nested functions
		- [x] Capturing used variables


- [ ] Debugging and Testing (consider developing robust tests for all part of the compiler)
	- [ ] Lexer error handling support
	- [ ] Parsing error handling support
	- [x] Compilation error handling support
		- [x] Custom Exception type
	- [ ] ~~Runtime error handling support~~

- [ ] Writing a report

## Notes

* Known bugs:
	1. Nothing that is not already mentioned as not done