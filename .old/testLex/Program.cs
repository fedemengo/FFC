using System;

namespace testLex
{
    class Program
    {
        static void Main(string[] args)
        {
            LexTrie lex = new LexTrie();
            
            lex.Add("if", "IF_STATEMENT");
            lex.Add("then", "THEN_STATEMENT");
            lex.Add("in", "IN_KEYWORD");
            
            foreach(string s in  lex.Parse("if if then infinnif$"))
            {
                Console.Write(s + " ");
            }
        }
    }
}
