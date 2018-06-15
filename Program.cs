using System;
using FFC.FLexer;
using FFC.FAST;
using FFC.FParser;
using FFC.FRunTime;

using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;


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
                if(Path.Substring(Path.Length-2) != ".f")
                    throw new FormatException("Can't compile non-.f file");
                Parser p = new Parser(new Scanner(new Tokenizer(), new SourceReader(Path)));
                bool res = p.Parse();
                Console.WriteLine($"\nParsing success : {res}\n");
                if (res)
                {
                    DeclarationStatementList stms = (DeclarationStatementList)p.GetAST();
                    //Print to see program's AST
                    stms.Print(0);
                    
                    //Generate PE compiling all statements   
                    string name = Path.Split('/')[1].Split('.')[0];
                    AppDomain appDomain = System.Threading.Thread.GetDomain();
                    AssemblyName asmName = new AssemblyName(name);

                    AssemblyBuilder asmBuilder = appDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
                    ModuleBuilder moduleBuilder = asmBuilder.DefineDynamicModule(asmName.Name, name + ".exe", true);

                    moduleBuilder.CreateGlobalFunctions();

                    /* Class 'Program' that will run all the statements */
                    TypeBuilder programType = moduleBuilder.DefineType("Program", TypeAttributes.Public);
                    ConstructorBuilder progConstr = programType.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                                                                CallingConventions.Standard,
                                                                new Type[0]);
                                    
                    ILGenerator progConstrGen = progConstr.GetILGenerator();
                    progConstrGen.Emit(OpCodes.Ldarg_0);
                    progConstrGen.Emit(OpCodes.Call, typeof(System.Object).GetConstructor(new Type[0]));
                    progConstrGen.Emit(OpCodes.Ret);
                    
                    /* Methods */
                    MethodBuilder mainMeth = programType.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, typeof(void), new Type[] { typeof(string[]) });

                    ILGenerator mainMethGen = mainMeth.GetILGenerator();
                    //generates all the statemetns in main
                    stms.Generate(mainMethGen);

                    mainMethGen.Emit(OpCodes.Ret);

                    // set assembly entry point
                    asmBuilder.SetEntryPoint(mainMeth);
                    // create program type
                    programType.CreateType();

                    asmBuilder.Save(name + ".exe");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\nCompilation completed successfully!");
                }
            }
            catch (NotImplementedException ex)
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
