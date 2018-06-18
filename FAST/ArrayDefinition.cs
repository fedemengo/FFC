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
    class ArrayDefinition : FPrimary
    {
        public void SetEmpty(FType t)
        {
            valueType = t;
        }
        public ExpressionList values;
        public ArrayDefinition(ExpressionList values, TextSpan span = null)
        {
            this.values = values;
            this.Span = span;
        }

        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Array definition");
            values.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            generator.Emit(OpCodes.Newobj, typeof(FArray<>).MakeGenericType((GetValueType(st) as ArrayType).type.GetRunTimeType()).GetConstructor(new Type[0]));
            foreach(var z in values.expressions)
            {
                z.Generate(generator, st);
                //I heard you liked long lines
                //generator.Emit(OpCodes.Newobj, typeof(FArray<>).MakeGenericType(z.ValueType(st).GetRunTimeType()).GetConstructor(new Type[]{typeof(FArray<>).MakeGenericType(z.ValueType(st).GetRunTimeType()), z.ValueType(st).GetRunTimeType()}));
                Type t = z.GetValueType(st).GetRunTimeType();
                Type a = typeof(FArray<>).MakeGenericType(t);
                generator.Emit(OpCodes.Newobj, a.GetConstructor(new Type[]{a, t}));
            }
        }

        public override void BuildType(SymbolTable st)
        {
            //empty arrays are already typed by SetEmpty, so we can throw if it happens here
            if(values == null || values.expressions == null || values.expressions.Count == 0)
                throw new NotImplementedException($"{Span} - Empty arrays can only be used in declarations/assignments, you pig!");
            valueType = values.expressions[0].GetValueType(st);
            foreach(var z in values.expressions)
                if(z.GetValueType(st).GetRunTimeType() != valueType.GetRunTimeType())
                    throw new NotImplementedException($"{Span} - Can't handle arrays with multiple types {valueType.GetType().Name} - {z.GetValueType(st).GetType().Name}");
            valueType = new ArrayType(valueType);
        }
        
    }
}