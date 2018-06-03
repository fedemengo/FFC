using System;
using FFC.FLexer;
using FFC.FAST;

namespace FFC.FParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //Scanner.Test(args[0]);
            //FLexerTTest.Test(args);
            Parser p = new Parser(new Scanner(new Tokenizer(), new SourceReader(args[0])));
            bool res = p.Parse();
            Console.WriteLine($"Parsing success : {res}");
            if(res)
            {
                FASTNode root = (FASTNode)p.GetAST();
                root.Print(0);
            }
        }
    }
}
