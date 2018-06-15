using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using FFC.FLexer;
using FFC.FParser;
using FFC.FRunTime;

namespace FFC.FAST
{
    class ArrayDefinition : FPrimary
    {
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
        public override void Generate(ILGenerator generator)
        {
            if(ValueType == null) throw new NotImplementedException($"{Span} - Emoty arrays are not implemented yet");
            generator.Emit(OpCodes.Newobj, typeof(FArray<>).MakeGenericType(values.expressions[0].ValueType.GetRunTimeType()).GetConstructor(new Type[0]));
            foreach(var z in values.expressions)
            {
                z.Generate(generator);
                //I heard you liked long lines
                //generator.Emit(OpCodes.Newobj, typeof(FArray<>).MakeGenericType(z.ValueType.GetRunTimeType()).GetConstructor(new Type[]{typeof(FArray<>).MakeGenericType(z.ValueType.GetRunTimeType()), z.ValueType.GetRunTimeType()}));
                Type t = z.ValueType.GetRunTimeType();
                Type a = typeof(FArray<>).MakeGenericType(t);
                generator.Emit(OpCodes.Newobj, a.GetConstructor(new Type[]{a, t}));
            }
        }

        public override void BuildType()
        {
            //how to handle when array is empty???
            if(values == null || values.expressions == null || values.expressions.Count == 0)
                ValueType = null;
            else
            {
                ValueType = values.expressions[0].ValueType;
                foreach(var z in values.expressions)
                    if(z.ValueType.GetType() != ValueType.GetType())
                    {
                        throw new NotImplementedException($"{Span} - Can't handle arrays with multiple types {ValueType.GetType().Name} - {z.ValueType.GetType().Name}");
                    }
                ValueType = new ArrayType(ValueType);
            }
        }
        
    }
}