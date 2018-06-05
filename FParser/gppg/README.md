# Parser creation instruction

This are the instructions to generate a working Parser from `grammar.y` using `gppg.exe`:

*   Using windows or wine, run `FParser/gppg/gppg.exe FParser/gppg/grammary.y > FParser/Parser.cs`
*   Running FParser/gppg/script.sh should generally be enough, but for details:
    *   If changes to token were made, update `FLexer/ETokens.cs` to match the ETokens enum inside the newly generated `FParser/Parser.cs` (except for the ERROR token that should be in upper case)
    *   In Parser.cs, make sure all the uses of the `error` token are refactored to `ERROR`, then remove the ETokens class from the source file

# Other information

*   Check ParserConstructor.cs for constructor declaration of Parser class
*   To see debug messages on Console, uncomment #define TRACE_ACTIONS in ShiftReduceParserCode
*   If public visibility is needed, uncomment #define EXPORT_GPPG in ShiftReduceParserCode
*   grammar.y contains instruction to generate the code with the right visibility (%internal) and to add the using of the proper namespaces (%using)