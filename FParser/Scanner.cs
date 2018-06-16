using System;
using FFC.FLexer;
using FFC.FGen;

namespace FFC.FParser
{
    internal class Scanner : QUT.Gppg.AbstractScanner<TValue, QUT.Gppg.LexLocation>
    {
        public Tokenizer lex;
        public SourceReader sr;
        public Token current;
        public Scanner(Tokenizer lex, SourceReader sr)
        {
            this.sr = sr;
            this.lex = lex;
        }
        public override int yylex()
        {
            current = lex.NextToken(sr);
            yylval = current.values;
            yylloc = new QUT.Gppg.LexLocation(current.begin.Row, current.begin.Column, current.end.Row, current.end.Column);
            return (int)current.type;
        }
        public static void Test(string path)
        {
            Tokenizer l = new Tokenizer();
            SourceReader sr = new SourceReader(path);
            Scanner s = new Scanner(l, sr);
            for(int i = 0; i < 10; i++)
                s.yylex();

        }
    }
}