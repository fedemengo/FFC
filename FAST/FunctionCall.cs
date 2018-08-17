using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using FFC.FParser;
using FFC.FGen;

namespace FFC.FAST
{
    public class FunctionCall : FSecondary
    {
        public FSecondary ToCall {get; set;}
        public ExpressionList ExprsList {get; set;}
        public FunctionCall(FSecondary toCall, ExpressionList exprs, TextSpan span)
        {
            ToCall = toCall;
            ExprsList = exprs;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Function call");
            ToCall.Print(tabs + 1);
            ExprsList.Print(tabs + 1);
        }
        public override void BuildValueType(SymbolTable st)
        {
            //Handles calls to standard functions
            if(ToCall is Identifier && StandardFunctions.Funcs.ContainsKey((ToCall as Identifier).Name))
            {
                ValueType = StandardFunctions.Funcs[(ToCall as Identifier).Name];
                return;
            }
            //Calls to source defined functions
            var t = ToCall.GetValueType(st);
            if(t is null) return;
            else if(t is FunctionType) ValueType = (t as FunctionType).ReturnType;
            else throw new FCompilationException($"{Span} - Can't call function on {ToCall.GetValueType(st)}.");
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            //Handles calls to standard functions
            if(ToCall is Identifier && StandardFunctions.Funcs.ContainsKey((ToCall as Identifier).Name))
            {
                switch((ToCall as Identifier).Name)
                {
                    case "length":
                        StandardFunctions.EmitLength(ExprsList, generator, currentType, st);
                        break;
                    case "round":
                        StandardFunctions.EmitRound(ExprsList, generator, currentType, st);
                        break;
                    case "rat":
                        StandardFunctions.EmitRat(ExprsList, generator, currentType, st);
                        break;
                    case "compl":
                        StandardFunctions.EmitCompl(ExprsList, generator, currentType, st);
                        break;
                    default:
                        throw new FCompilationException($"{Span} - Standard function {(ToCall as Identifier).Name} not implemented yet");
                }
                return;
            }
            //Calls to source defined functions
            if(ToCall.GetValueType(st) is FunctionType == false)
                throw new FCompilationException($"{Span} - Can't call function on {ToCall.GetValueType(st)}.");

            FunctionType funcType = ToCall.GetValueType(st) as FunctionType;
            TypeBuilder funcTypeBuilder = Generator.FunctionTypes[funcType];

            if(funcType.ParamsList.Types.Count != ExprsList.Exprs.Count)
                throw new FCompilationException($"{Span} - Parameter count mismatch on {ToCall.GetValueType(st)}.");

            ToCall.Generate(generator, currentType, st, exitLabel, conditionLabel);
            List<Type> paramTypes = new List<Type>();
            for(int i=0; i<ExprsList.Exprs.Count; ++i)
            {
                FType exprFType = ExprsList.Exprs[i].GetValueType(st);

                if(!FType.SameType(funcType.ParamsList.Types[i], exprFType))
                    throw new FCompilationException($"{Span} - Parameter {i} should be {funcType.ParamsList.Types[i].ToString()} instead of {exprFType.ToString()}.");
                paramTypes.Add(exprFType.GetRunTimeType());
                ExprsList.Exprs[i].Generate(generator, currentType, st);
            }
            
            generator.Emit(OpCodes.Callvirt, funcTypeBuilder.GetMethod("Invoke", paramTypes.ToArray()));
        }
    }
}
