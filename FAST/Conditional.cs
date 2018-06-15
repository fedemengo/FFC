using System;
using System.Collections.Generic;
using FFC.FParser;
using FFC.FRunTime;
using System.Reflection.Emit;
using System.Reflection;

namespace FFC.FAST
{
    class Conditional : FPrimary
    {
        public FExpression condition;
        public FExpression ifTrue;
        public FExpression ifFalse;
        public Conditional(FExpression condition, FExpression ifTrue, FExpression ifFalse, TextSpan span)
        {
            this.condition = condition;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
            this.Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Conditional expression");
            condition.Print(tabs + 1);
            ifTrue.Print(tabs + 1);
            ifFalse.Print(tabs + 1);
        }

        public override void BuildType() 
        {
            if(ifTrue.ValueType.GetRunTimeType() == ifFalse.ValueType.GetRunTimeType())
                ValueType = ifTrue.ValueType;
            else
                throw new NotImplementedException($"{Span} - Different type in conditional expression");
        }
        public override void Generate(System.Reflection.Emit.ILGenerator generator)
        {
            if(condition.ValueType.GetRunTimeType() != typeof(FBoolean))
            {
                throw new NotImplementedException($"Can't use conditional with {condition.ValueType}");
            }
            condition.Generate(generator);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
            Label falseBranch = generator.DefineLabel();
            Label exitBranch = generator.DefineLabel();
            generator.Emit(OpCodes.Brfalse, falseBranch);
            ifTrue.Generate(generator);
            generator.Emit(OpCodes.Br, exitBranch);
            generator.MarkLabel(falseBranch);
            ifFalse.Generate(generator);
            generator.MarkLabel(exitBranch);
        }
    }
}