using System;
using System.Collections.Generic;

namespace Lexer
{
    class FLexerTest
    {
        static void Main(string[] args)
        {
            SourceReader code = new SourceReader(args[0]);
            FLexer lexer = new FLexer();
            List<Token> tokens = lexer.GetTokens(code);
            foreach(Token t in tokens)
                Console.WriteLine(t.ToString());
        }
    }
}
