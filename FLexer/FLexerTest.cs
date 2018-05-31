using System;
using System.Collections.Generic;

namespace FFC.FLexer
{
    class FLexerTest
    {
        static public void Test(string[] args)
        {
            SourceReader code = new SourceReader(args[0]);
            FLexer lexer = new FLexer();
            List<Token> tokens = lexer.GetTokens(code);
            foreach(Token t in tokens)
                Console.WriteLine(t.ToString());
        }
    }
}
