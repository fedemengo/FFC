using System;
using FFC.FLexer;
using FFC.FAST;

using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;


namespace FFC.FParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //Scanner.Test(args[0]);
            //FLexerTTest.Test(args);
            string Path = "samples/declaration.f";
            FFC.FParser.Parser p = new FFC.FParser.Parser(new Scanner(new Tokenizer(), new SourceReader(Path)));
            bool res = p.Parse();
            Console.WriteLine($"\nParsing success : {res}\n");
            if (res)
            {
                DeclarationStatementList stms = (DeclarationStatementList)p.GetAST();
                //Print to see program's AST
                stms.Print(0);
                

                //Generate PE compiling all statements
                
                string name = Path.Split('/')[1].Split('.')[0];
                AppDomain ad = System.Threading.Thread.GetDomain();
                AssemblyName an = new AssemblyName(name);

                AssemblyBuilder ab = ad.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndSave);
                ModuleBuilder modb = ab.DefineDynamicModule(an.Name, name + ".exe", true);

                modb.CreateGlobalFunctions();

                /* Class 'Program' that will run all the statements */
                TypeBuilder tb = modb.DefineType("Program", TypeAttributes.Public);
                ConstructorBuilder cb = tb.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                                                            CallingConventions.Standard,
                                                            new Type[0]);
                                 
                ILGenerator ilConstr = cb.GetILGenerator();
                ilConstr.Emit(OpCodes.Ldarg_0);
                
                ilConstr.Emit(OpCodes.Call, typeof(System.Object).GetConstructor(new Type[0]));
                ilConstr.Emit(OpCodes.Ret);

                /* Methods */
                MethodBuilder mb = tb.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, typeof(void), new Type[] { typeof(string[]) });

                ILGenerator ilMeth = mb.GetILGenerator();

                //generates all the statemetns in main
                stms.Generate(ilMeth);

                ilMeth.Emit(OpCodes.Ret);

                ab.SetEntryPoint(mb);

                tb.CreateType();

                ab.Save(name + ".exe");
            }
        }
    }
}
