using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

using FFC.FAST;
using FFC.FGen;

namespace FFC.FGen
{
    public partial class Generator
    {
        //call reset to have it start compilation of a new file
        public static void Reset() => throw new NotImplementedException("Reset of the generator status is not currently implemented");
        
        //stored globally to emit delegate types for function types
        private static TypeBuilder programType;

        //might we need to store more stuff globally ?

        public static bool Generate(string Path, DeclarationStatementList stms)
        {
            
            string name = Path.Split('/')[1].Split('.')[0];
            AppDomain appDomain = System.Threading.Thread.GetDomain();
            AssemblyName asmName = new AssemblyName(name);

            AssemblyBuilder asmBuilder = appDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = asmBuilder.DefineDynamicModule(asmName.Name, name + ".exe", true);

            moduleBuilder.CreateGlobalFunctions();

            /* Class 'Program' that will run all the statements */
            
            programType = moduleBuilder.DefineType("Program", TypeAttributes.Public);
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
            stms.Generate(mainMethGen, programType, new SymbolTable());

            mainMethGen.Emit(OpCodes.Ret);

            // set assembly entry point
            asmBuilder.SetEntryPoint(mainMeth);
            // create program type
            programType.CreateType();

            asmBuilder.Save(name + ".exe");

            return true;
        }
        public static Dictionary<FunctionType, TypeBuilder> FunctionTypes;
        public static void AddFunctionType(FunctionType f)
        {
            throw new NotImplementedException($"I don't know how to emit delegate types");
        }
        private static int funcCount = 0;
        private static string GetNextFuncName() => "___f" + funcCount.ToString();
        public static TypeBuilder GetFunction(TypeBuilder parentType, FunctionType funcType)
        {
            if(FunctionTypes.ContainsKey(funcType) == false)
                AddFunctionType(funcType);
            throw new NotImplementedException($"I don't know how to emit function types of some given deleagates");
        }
    }
}