using System;
using System.Collections.Generic;

namespace FFC.FLexer
{
    class FLexerTest
    {
        static public void Test(string[] args)
        {
            SourceReader code = new SourceReader(args[0]);
            Tokenizer lexer = new Tokenizer();
            List<Token> tokens = lexer.GetTokens(code);
            foreach(Token t in tokens)
                Console.WriteLine(t.ToString());
        }
    }
}
