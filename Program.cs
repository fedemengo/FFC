﻿using System;
using FFC.FLexer;
using FFC.FAST;
using FFC.FParser;
using FFC.FRunTime;
using FFC.FGen;

namespace FFC.FParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //tmp fix for console color not adjusting
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                string Path = args[0];
                string funcName = args.Length >= 2 ? args[1] : "main";
                if(Path.Substring(Path.Length-2) != ".f")
                    throw new FormatException("Can't compile non-.f file");
                Parser parser = new Parser(new Scanner(new Tokenizer(), new SourceReader(Path)));
                bool res = parser.Parse();
                Console.WriteLine($"\nParsing success : {res}\n");
                if (res)
                {
                    DeclarationStatementList stms = (DeclarationStatementList) parser.GetAST();
                    //Print to see program's AST
                    stms.Print(0);
                    
                    //Generate PE compiling all statements   
                    Generator.Generate(Path, stms, funcName);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nCompilation completed successfully!");
                }
            }
            catch (FCompilationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nCOMPILATION FAILED :");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine();
            }
        }
    }
}
