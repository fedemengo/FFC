using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using FFC.FLexer;
using FFC.FParser;
using FFC.FRunTime;
using FFC.FGen;

namespace FFC.FAST
{
    public class ArrayDefinition : FPrimary
    {
        public void SetEmpty(FType t)
        {
            ValueType = t;
        }
        public ExpressionList ExprsList;
        public ArrayDefinition(ExpressionList values, TextSpan span = null)
        {
            ExprsList = values;
            Span = span;
        }

        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Array definition");
            ExprsList.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            generator.Emit(OpCodes.Newobj, typeof(FArray<>).MakeGenericType((GetValueType(st) as ArrayType).Type.GetRunTimeType()).GetConstructor(new Type[0]));
            foreach(var expr in ExprsList.Exprs)
            {
                expr.Generate(generator, currentType, st, exitLabel, conditionLabel);
                Type exprType = expr.GetValueType(st).GetRunTimeType();
                Type exprGenType = typeof(FArray<>).MakeGenericType(exprType);
                generator.Emit(OpCodes.Newobj, exprGenType.GetConstructor(new Type[]{exprGenType, exprType}));
            }
        }

        public override void BuildValueType(SymbolTable st)
        {
            //empty arrays are already typed by SetEmpty, so we can throw if it happens here
            if(ExprsList == null || ExprsList.Exprs == null || ExprsList.Exprs.Count == 0)
                throw new NotImplementedException($"{Span} - Empty arrays can only be used in declarations/assignments, you pig!");
            ValueType = ExprsList.Exprs[0].GetValueType(st);
            foreach(var z in ExprsList.Exprs)
                if(FType.SameType(z.GetValueType(st), ValueType) == false)
                    throw new NotImplementedException($"{Span} - Can't handle arrays with multiple types {ValueType.GetType().Name} - {z.GetValueType(st).GetType().Name}");
            ValueType = new ArrayType(ValueType);
        }
    }
}