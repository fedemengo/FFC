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
    public class LoopStatement : FStatement
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
            if(header != null)
                header.Print(tabs + 1);
            body.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            Label loopCondition = generator.DefineLabel();
            Label exitLabel = generator.DefineLabel();
            //loop condition is generated inside FLoopHeader
            if(header != null)
                header.Generate(generator, loopCondition, exitLabel, ref st);
            else
                generator.MarkLabel(loopCondition);
            body.Generate(generator, loopCondition, exitLabel, st);
            generator.Emit(OpCodes.Br, loopCondition);
            generator.MarkLabel(exitLabel);
        }
    }
    public abstract class FLoopHeader : FASTNode
    {
        //A loop header has to jump to exitLabel if false, and has to Mark condLabel
        public virtual void Generate(ILGenerator generator, Label condLabel, Label exitLabel, ref SymbolTable st)
        {
            throw new NotImplementedException($"{Span} - Generation not implemented for {GetType().Name}");
        }
    }
    public class ForHeader : FLoopHeader
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

        public override void Generate(ILGenerator generator, Label condLabel, Label exitLabel, ref SymbolTable st)
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
            
            // Create lb for iterator
            LocalBuilder it = generator.DeclareLocal(typeof(FIterator<>).MakeGenericType(collValueRTType));
            generator.Emit(OpCodes.Stloc, it);
            // If header has id, loads it
            if(id != null)
            {
                //assign new Local to id in SymbolTable 
                st = st.Assign(id.name, new NameInfo(generator.DeclareLocal(collValueRTType), iterableType.type));
            }

            // Advance iterator
            generator.MarkLabel(condLabel); // Has to repeat from here
            generator.Emit(OpCodes.Ldloc, it);
            generator.Emit(OpCodes.Callvirt, typeof(FIterator<>).MakeGenericType(collValueRTType).GetMethod("MoveNext"));
            //If false (reached it end) exit from loop
            generator.Emit(OpCodes.Brfalse, exitLabel);
            //if id was set, updates it
            if(id != null)
            {
                generator.Emit(OpCodes.Ldloc, it);
                //get value
                generator.Emit(OpCodes.Callvirt, typeof(FIterator<>).MakeGenericType(collValueRTType).GetMethod("GetCurrent"));
                //assign it to "iterating" variable
                generator.Emit(OpCodes.Stloc, st.Find(id.name).LocBuilder);
            }
        }
    }
    public class WhileHeader : FLoopHeader
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

        public override void Generate(ILGenerator generator, Label condLabel, Label exitLabel, ref SymbolTable st)
        {
            generator.MarkLabel(condLabel); //While repeats from condition check
            if(condition.GetValueType(st).GetRunTimeType() != typeof(FBoolean))
                throw new NotImplementedException($"{Span} - Can't use conditional with {condition.GetValueType(st)}");
            condition.Generate(generator, st);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
            generator.Emit(OpCodes.Brfalse, exitLabel);
        }
    }
}