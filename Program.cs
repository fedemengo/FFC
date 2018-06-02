using System;
using FFC.FLexer;
using FFC.FParser;

namespace FFC.FParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser p = new Parser(new Scanner(new Tokenizer(), new SourceReader(args[0])));
        }
    }
}
