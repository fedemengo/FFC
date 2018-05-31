using System;
using FFC.FLexer;

namespace FFC
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 0) FLexerTest.Test(args);
            else
            {
                var s = Console.ReadLine();
                string[] a = {s};
                FLexerTest.Test(a);
            }
        }
    }
}
