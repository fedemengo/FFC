using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;

using FFC.FParser;
using FFC.FRunTime;
using FFC.FGen;

namespace FFC.FAST
{
    class LoopStatement : FStatement
    {
        public FLoopHeader header;
        public StatementList body;

        public LoopStatement(FLoopHeader header, StatementList body, TextSpan span)
        {
            this.Span = span;
            this.header = header;
            this.body = body;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Loop statement");
            header.Print(tabs + 1);
            body.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            Label loopCondition = generator.DefineLabel();
            Label exitLabel = generator.DefineLabel();
            generator.MarkLabel(loopCondition);
            header.Generate(generator, exitLabel, st);
            body.Generate(generator, loopCondition, exitLabel, st);
            generator.Emit(OpCodes.Br, loopCondition);
            generator.MarkLabel(exitLabel);
        }
    }
    abstract class FLoopHeader : FASTNode
    {
        public virtual void Generate(ILGenerator generator, Label nextLabel, SymbolTable st)
        {
            throw new NotImplementedException($"{Span} - Generation not implemented for {GetType().Name}");
        }
    }
    class ForHeader : FLoopHeader
    {
        public Identifier id;
        public FExpression collection;

        public ForHeader(Identifier id, FExpression collection, TextSpan span)
        {
            this.Span = span;
            this.id = id;
            this.collection = collection;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("For header");
            if(id != null) id.Print(tabs + 1);
            collection.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, Label exitLabel, SymbolTable st)
        {
            // Generate iterator
            FType collType = collection.GetValueType(st);
            IterableType iterableType = collType as IterableType;
            Type collValueRTType = iterableType.type.GetRunTimeType();
            
            if(iterableType == null)
                throw new NotImplementedException($"{Span} - Can't iterate on {collType}");
            collection.Generate(generator, st);
            if(!collType.GetRunTimeType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(FIterable<>)))
                throw new NotImplementedException($"{Span} - {collType.GetRunTimeType().Name} is not FIterable {iterableType.type.GetRunTimeType().Name}");
            generator.Emit(OpCodes.Callvirt, collType.GetRunTimeType().GetMethod("GetIterator"));
            
            // Save iterator
            LocalBuilder it = generator.DeclareLocal(typeof(FIterator<>).MakeGenericType(collValueRTType));
            generator.Emit(OpCodes.Stloc, it);
            Label condition = generator.DefineLabel();
            generator.MarkLabel(condition);

            
        }
    }
    class WhileHeader : FLoopHeader
    {
        public FExpression condition;

        public WhileHeader(FExpression condition, TextSpan span)
        {
            this.Span = span;
            this.condition = condition;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("While header");
            condition.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, Label exitLabel, SymbolTable st)
        {
            if(condition.GetValueType(st).GetRunTimeType() != typeof(FBoolean))
            {
                throw new NotImplementedException($"{Span} - Can't use conditional with {condition.GetValueType(st)}");
            }
            condition.Generate(generator, st);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
            generator.Emit(OpCodes.Brfalse, exitLabel);
        }
    }
}