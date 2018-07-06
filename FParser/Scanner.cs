using System;
using FFC.FLexer;
using FFC.FGen;
using QUT.Gppg;

namespace FFC.FParser
{
    internal class Scanner : AbstractScanner<TValue, LexLocation>
    {
        public Tokenizer tokenizer;
        public SourceReader reader;
        public Token current;
        
        public Scanner(Tokenizer tokz, SourceReader rd)
        {
            tokenizer = tokz;
            reader = rd;
        }
        
        public override int yylex()
        {
            current = tokenizer.NextToken(reader);
            yylval = current.values;
            yylloc = new LexLocation(current.Begin.Row, current.Begin.Column, current.End.Row, current.End.Column);
            return (int) current.Type;
        }
    }
}