using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using FFC.FParser;
using FFC.FGen;
using FFC.FRunTime;

namespace FFC.FAST
{
    public abstract class FExpression : FASTNode
    {
        /*
            inherited by
                BinOpExpr
                NegExpr
                EllipsisExpr
                FSecondary
        */
        public virtual void EmitPrint(ILGenerator generator, TypeBuilder currentType, SymbolTable st, bool newLine = false)
        {
            Generate(generator, currentType, st);
            FType t = this is Identifier ? st.Find((this as Identifier).Name).Type : GetValueType(st);
            generator.Emit(OpCodes.Call, typeof(System.Console).GetMethod(newLine ? "WriteLine" : "Write", new Type[]{t.GetType()}));
        }
    }
    public class ExpressionList : FASTNode
    {
        public List<FExpression> Exprs {get; set;}
        public ExpressionList(FExpression expr, TextSpan span)
        {
            Exprs = new List<FExpression>{expr};
            Span = span;
        }
        public ExpressionList(TextSpan span = null)
        {
            Exprs = new List<FExpression>();
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Expression list");
            foreach(FExpression e in Exprs)
                e.Print(tabs + 1);
        }
    }
    public class BinaryOperatorExpression : FExpression
    {
        public FExpression Left {get; set;}
        public FOperator BinOperator {get; set;}
        public FExpression Right {get; set;}
        public BinaryOperatorExpression(FExpression left, FOperator binOperator, FExpression right, TextSpan span)
        {
            Left = left;
            BinOperator = binOperator;
            Right = right;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Binary operator");
            Left.Print(tabs+1);
            BinOperator.Print(tabs+1);
            Right.Print(tabs+1);
        }
        public override void BuildValueType(SymbolTable st) => ValueType = BinOperator.GetTarget(Left.GetValueType(st), Right.GetValueType(st));

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            GetValueType(st);
            if(ValueType is MapType)
                throw new FCompilationException(Span + " - Operations on maps are not yet implemented.");
            if(ValueType is TupleType)
                throw new FCompilationException(Span + " - Operations on tuples are not yet implemented.");
            
            FType targetType = ValueType;
            
            if(BinOperator is RelationalOperator)
            {
                //we need to cast to the 2same type they would get summed to
                if(Left.GetValueType(st) is BooleanType && Right.GetValueType(st) is BooleanType)
                    targetType = new EqualOperator(null).GetTarget(Left.GetValueType(st), Right.GetValueType(st));
                else
                    targetType = new PlusOperator(null).GetTarget(Left.GetValueType(st), Right.GetValueType(st));
            }
            Left.Generate(generator, currentType, st, exitLabel, conditionLabel);
            if(FType.SameType(Left.GetValueType(st), targetType) == false)
                Left.GetValueType(st).ConvertTo(targetType, generator);
            
            Label right = generator.DefineLabel();
            Label skip = generator.DefineLabel();

            //For AndOperator, we need to add conditional jump to the end if result was False
            if(BinOperator is AndOperator)
            {
                //if true, valuate right
                generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("GetBool"));
                generator.Emit(OpCodes.Brtrue, right);
                //if false, re-emit FBoolean(false) and skip
                generator.Emit(OpCodes.Ldc_I4_0);
                generator.Emit(OpCodes.Newobj, typeof(FBoolean).GetConstructor(new Type[]{typeof(bool)}));
                generator.Emit(OpCodes.Br, skip);
            }
            //For OrOperator, we can skip if result is True
            else if(BinOperator is OrOperator)
            {
                //if false, valuate right
                generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("GetBool"));
                generator.Emit(OpCodes.Brtrue, right);
                //if true, re-emit FBoolean(true) and skip
                generator.Emit(OpCodes.Ldc_I4_1);
                generator.Emit(OpCodes.Newobj, typeof(FBoolean).GetConstructor(new Type[]{typeof(bool)}));
                generator.Emit(OpCodes.Br, skip);
            }

            //Label to valuate second
            generator.MarkLabel(right);

            Right.Generate(generator, currentType, st, exitLabel, conditionLabel);
            if(FType.SameType(Right.GetValueType(st),targetType) == false)
                Right.GetValueType(st).ConvertTo(targetType, generator);

            //Label to end statement
            generator.MarkLabel(skip);

            //We implicitly solved operator already
            if(BinOperator is AndOperator || BinOperator is OrOperator)
                return;

            string op_name = BinOperator.GetMethodName();
            Type rtt = targetType.GetRunTimeType();
            //this goes for binOperator.Generate();
            generator.Emit(OpCodes.Call, rtt.GetMethod(op_name, new Type[]{rtt, rtt}));
        }
    }
    public class NegativeExpression : FExpression
    {
        public FSecondary Value {get; set;}
        public NegativeExpression(FSecondary value, TextSpan span)
        {
            Value = value;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Negative expression");
            Value.Print(tabs + 1);
        }

        public override void BuildValueType(SymbolTable st) => ValueType = Value.GetValueType(st);

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            Value.Generate(generator, currentType, st, exitLabel, conditionLabel);
            //we call -(obj) for ValueType
            generator.Emit(OpCodes.Call, Value.GetValueType(st).GetRunTimeType().GetMethod("op_UnaryNegation", new Type[]{ValueType.GetRunTimeType()}));
        }
    }
    public class EllipsisExpression : FExpression
    {
        public FSecondary From {get; set;}
        public FSecondary To {get; set;}
        public EllipsisExpression(FSecondary from, FSecondary to, TextSpan span)
        {
            From = from;
            To = to;
            Span = span;
        }
        public override void BuildValueType(SymbolTable st)
        {
            //Only integers are allowed for ellipsis
            if(FType.SameType(From.GetValueType(st), To.GetValueType(st)) == false ||
               FType.SameType(From.GetValueType(st), new IntegerType()) == false)
                throw new FCompilationException($"{Span} - Can't use ellipsis with {From.GetValueType(st)}-{To.GetValueType(st)}");
            ValueType = new EllipsisType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Ellipsis expression");
            From.Print(tabs + 1);
            To.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            From.Generate(generator, currentType, st, exitLabel, conditionLabel);
            To.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Newobj, typeof(FEllipsis).GetConstructor(new Type[]{typeof(FInteger), typeof(FInteger)}));
        }
    }
    public class NotExpression : FExpression
    {
        public FExpression Expr {get; set;}
        public NotExpression(FExpression expr, TextSpan span)
        {
            Span = span;
            Expr = expr;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Not expression");
            Expr.Print(tabs + 1);
        }
        public override void BuildValueType(SymbolTable st) => ValueType = Expr.GetValueType(st);

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            Expr.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Call, Expr.GetValueType(st).GetRunTimeType().GetMethod("op_LogicalNot", new Type[]{Expr.GetValueType(st).GetRunTimeType()}));
        }
    }

    public class ReadExpression : FExpression
    {
        public FType Type {get; set;}
        public ReadExpression(FType type)
        {
            if(type == null) throw new FCompilationException($"{Span} - Can't use read keyword without specifying type");
            if(type.GetRunTimeType().GetMethod("Read") == null) throw new FCompilationException($"{Span} - Read does not support {type}");
            Type = type;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Read Expression");
            Type.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            //Idea is to use runtime function Read(), so that everything depends on library implementation of types
            generator.Emit(OpCodes.Call, Type.GetRunTimeType().GetMethod("Read"));
        }
        public override void BuildValueType(SymbolTable st) => ValueType = Type;
    }
}