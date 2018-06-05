# FFC
A compiler for the F language (and it's definition)

## TODOs
* Fix names in grammar
* Action on the tree 
	* Code generation (e.g. If is smth in stack and some conditional jump) (recursively within different statement)
	* Reorganize tree (optimization)
* Mappings between current and final code
* Generate code for simple program (e.g "hello world" and look into it with **ildasm.exe**)
* Dynamically load external library 

* Debugging on AST printing
* Think if more ```\``` escaped character are needed
* Think about ```\n``` instead of ```;``` for SEMICOLON tokens

* Handling error token

## Progress

- [x] Lexical analyser
	
- [x] Parser
	- [x] Grammar	
	- [x] Semantic actions
	- [x] AST structure

- [ ] Mappings (main constructs, nested functions)

- [ ] Code generation (while creating mappings)

- [ ] Debugging and Testing

- [ ] Writing a report


