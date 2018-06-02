using System;
using FFC.FLexer;

namespace FFC.FParser
{
    internal class Scanner : QUT.Gppg.AbstractScanner<TValue, QUT.Gppg.LexLocation>
    {
        public Tokenizer lex;
        public SourceReader sr;
        public Token current;
        protected Scanner(Tokenizer lex, SourceReader sr)
        {
            this.sr = sr;
            this.lex = lex;
        }
        public override int yylex()
        {
            current = lex.NextToken(sr);
            yylval = current.values;
            yylloc = current.span;
            return (int)current.type;
        }
    }
}