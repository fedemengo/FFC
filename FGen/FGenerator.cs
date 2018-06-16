using System;
using System.Reflection;
using System.Reflection.Emit;

using FFC.FAST;
using FFC.FGen;

namespace FFC.FGen
{
    class Generator
    {
        public static bool Generate(string Path, DeclarationStatementList stms)
        {
            
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
            //generates all the statements in main
            stms.Generate(mainMethGen, new SymbolTable());

            mainMethGen.Emit(OpCodes.Ret);

            // set assembly entry point
            asmBuilder.SetEntryPoint(mainMeth);
            // create program type
            programType.CreateType();

            asmBuilder.Save(name + ".exe");

            return true;
        }
    }
}