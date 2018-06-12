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
            //tmp fix for console color not adjusting
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                //Scanner.Test(args[0]);
                //FLexerTTest.Test(args);
                //string Path = "samples/declaration.f";
                string Path = args[0];
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
                    
                    ConstructorBuilder ratConstr = CreateRational(moduleBuilder);
                    
                    mainMethGen.Emit(OpCodes.Ldc_I4_S, 41);
                    mainMethGen.Emit(OpCodes.Ldc_I4_S, 84);
                    mainMethGen.Emit(OpCodes.Newobj, ratConstr);
                    //ilMeth.Emit(OpCodes.Stloc_0);
                    //ilMeth.Emit(OpCodes.Ldloc_0);
                    mainMethGen.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new Type[]{typeof(object)}));

                    mainMethGen.Emit(OpCodes.Nop);
                    //ilMeth.Emit(OpCodes.Ldc_I4_S, 123);
                    //ilMeth.Emit(OpCodes.Ldc_I4_S, 8);
                    //ilMeth.Emit(OpCodes.Newobj, ratConstr);
                    //ilMeth.Emit(OpCodes.Stloc_1);

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
            catch (Exception ex)
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
        static ConstructorBuilder CreateRational(ModuleBuilder moduleBuilder)
        {
            TypeBuilder rational = moduleBuilder.DefineType("Rational", TypeAttributes.Public);
            
            // Fields
            FieldBuilder fNum = rational.DefineField("num", typeof(int), FieldAttributes.Public);
            FieldBuilder fDen = rational.DefineField("den", typeof(int), FieldAttributes.Public);
            
            // Methods
            /*
                MethodBuilder GCD = rational.DefineMethod("GCD", MethodAttributes.Private
                                                                    | MethodAttributes.Static
                                                                    | MethodAttributes.HideBySig, CallingConventions.Standard, typeof(int), new Type[]{typeof(int), typeof(int)});
                ILGenerator GCDGen = GCD.GetILGenerator();
            
                Label branchFalse = GCDGen.DefineLabel();
                
                GCDGen.Emit(OpCodes.Ldarg_0);
                GCDGen.Emit(OpCodes.Ldc_I4, 0);
                GCDGen.Emit(OpCodes.Stloc_0);
                GCDGen.Emit(OpCodes.Ldloc_0);
                GCDGen.Emit(OpCodes.Bne_Un, branchFalse);
                GCDGen.Emit(OpCodes.Ldarg_0);
                GCDGen.Emit(OpCodes.Stloc_1);
            */

                MethodBuilder toString = rational.DefineMethod("ToString", MethodAttributes.Public
                                                                            | MethodAttributes.HideBySig
                                                                            | MethodAttributes.Virtual, CallingConventions.Standard, typeof(string), new Type[0]);
            
            ILGenerator toStringGen = toString.GetILGenerator();
            toStringGen.Emit(OpCodes.Ldarg_0);
            toStringGen.Emit(OpCodes.Ldfld, fNum);
            toStringGen.Emit(OpCodes.Box, typeof(int));
            toStringGen.Emit(OpCodes.Ldstr, "/");
            toStringGen.Emit(OpCodes.Ldarg_0);
            toStringGen.Emit(OpCodes.Ldfld, fDen);
            toStringGen.Emit(OpCodes.Box, typeof(int));
            toStringGen.Emit(OpCodes.Call, typeof(System.String).GetMethod("Concat", new Type[]{typeof(object), typeof(object), typeof(object)}));
            //toStringGen.Emit(OpCodes.Stloc_0);
            //toStringGen.Emit(OpCodes.Ldloc_0);
            toStringGen.Emit(OpCodes.Ret);

            // Add
            /*
                MethodBuilder Add = rational.DefineMethod("Add", MethodAttributes.Public
                                                                | MethodAttributes.HideBySig
                                                                | MethodAttributes.Virtual, CallingConventions.Standard, typeof(string), new Type[0]);
             */
            

            // Constructor
            ConstructorBuilder ratConstr = rational.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard, new Type[] { typeof(System.Int32), typeof(System.Int32) });

            ILGenerator genConstr = ratConstr.GetILGenerator();
            genConstr.Emit(OpCodes.Ldarg_0);
            genConstr.Emit(OpCodes.Call, typeof(System.Object).GetConstructor(new Type[0]));
            genConstr.Emit(OpCodes.Nop);
            genConstr.Emit(OpCodes.Nop);
            genConstr.Emit(OpCodes.Ldarg_0);
            genConstr.Emit(OpCodes.Ldarg_1);
            genConstr.Emit(OpCodes.Stfld, fNum);
            genConstr.Emit(OpCodes.Ldarg_0);
            genConstr.Emit(OpCodes.Ldarg_2);
            genConstr.Emit(OpCodes.Stfld, fDen);
            genConstr.Emit(OpCodes.Ret);

            rational.CreateType();
            
            return ratConstr;

            /*

            // Type
            TypeBuilder rational = moduleBuilder.DefineType("Rational", TypeAttributes.Public);
            
            // Fields
            // num property
            FieldBuilder fNum = rational.DefineField("num", typeof(System.Int32), FieldAttributes.Private);
            //PropertyBuilder pNum = rational.DefineProperty("Num", PropertyAttributes.HasDefault, typeof(System.Int32), null);

            //MethodBuilder NumGet = rational.DefineMethod("get_Num", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(System.Int32), new Type[0]);
            //ILGenerator getNumGen = NumGet.GetILGenerator();
            //getNumGen.Emit(OpCodes.Ldarg_0);
            //getNumGen.Emit(OpCodes.Ldfld, fNum);
            //getNumGen.Emit(OpCodes.Ret);

            //MethodBuilder NumSet = rational.DefineMethod("set_Num", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new Type[] { typeof(System.Int32)});
            //ILGenerator setNumGen = NumSet.GetILGenerator();
            //setNumGen.Emit(OpCodes.Ldarg_0);
            //setNumGen.Emit(OpCodes.Ldarg_1);
            //setNumGen.Emit(OpCodes.Stfld, fNum);
            //setNumGen.Emit(OpCodes.Ret);

            //pNum.SetGetMethod(NumGet);
            //pNum.SetSetMethod(NumSet);

            // den property
            FieldBuilder fDen = rational.DefineField("den", typeof(System.Int32), FieldAttributes.Private);
            //PropertyBuilder pDen = rational.DefineProperty("Den", PropertyAttributes.HasDefault, typeof(System.Int32), null);

            //MethodBuilder DenGet = rational.DefineMethod("get_Den", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(System.Int32), new Type[0]);
            //ILGenerator getDenGen = DenGet.GetILGenerator();
            //getDenGen.Emit(OpCodes.Ldarg_0);
            //getDenGen.Emit(OpCodes.Ldfld, fNum);
            //getDenGen.Emit(OpCodes.Ret);

            //MethodBuilder DenSet = rational.DefineMethod("set_Den", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new Type[] { typeof(System.Int32) });
            //ILGenerator setDenGen = DenSet.GetILGenerator();
            //setDenGen.Emit(OpCodes.Ldarg_0);
            //setDenGen.Emit(OpCodes.Ldarg_1);
            //setDenGen.Emit(OpCodes.Stfld, fNum);
            //setDenGen.Emit(OpCodes.Ret);

            //pDen.SetGetMethod(DenGet);
            //pDen.SetSetMethod(DenSet);

            // Constructor
            ConstructorBuilder ratConstr = rational.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard, new Type[] { typeof(System.Int32), typeof(System.Int32) });

            ILGenerator genConstr = ratConstr.GetILGenerator();
            genConstr.Emit(OpCodes.Ldarg_0);
            genConstr.Emit(OpCodes.Call, typeof(System.Object).GetConstructor(new Type[0]));
            genConstr.Emit(OpCodes.Nop);
            genConstr.Emit(OpCodes.Nop);
            genConstr.Emit(OpCodes.Ldarg_0);
            genConstr.Emit(OpCodes.Ldarg_1);
            genConstr.Emit(OpCodes.Stfld, fNum);
            genConstr.Emit(OpCodes.Ldarg_0);
            genConstr.Emit(OpCodes.Ldarg_2);
            genConstr.Emit(OpCodes.Stfld, fDen);
            genConstr.Emit(OpCodes.Ret);

            rational.CreateType();

            */
        }
    }
}
