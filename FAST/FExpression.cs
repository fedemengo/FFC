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
            FType t = this is Identifier ? st.Find((this as Identifier).name).Type : GetValueType(st);
            generator.Emit(OpCodes.Call, typeof(System.Console).GetMethod(newLine ? "WriteLine" : "Write", new Type[]{t.GetType()}));
        }
    }
    public class ExpressionList : FASTNode
    {
        public List<FExpression> expressions;
        public ExpressionList(FExpression expr, TextSpan span)
        {
            this.Span = span;
            expressions = new List<FExpression>{expr};
        }
        public ExpressionList(TextSpan span = null)
        {
            this.Span = span;
            expressions = new List<FExpression>();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Expression list");
            foreach(FExpression e in expressions)
                e.Print(tabs + 1);
        }
    }
    public class BinaryOperatorExpression : FExpression
    {
        public FExpression left;
        public FOperator binOperator;
        public FExpression right;
        public BinaryOperatorExpression(FExpression left, FOperator binOperator, FExpression right, TextSpan span)
        {
            this.Span = span;
            this.left = left;
            this.binOperator = binOperator;
            this.right = right;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Binary operator");
            left.Print(tabs+1);
            binOperator.Print(tabs+1);
            right.Print(tabs+1);
        }
        public override void BuildType(SymbolTable st)
        {
            valueType = binOperator.GetTarget(left.GetValueType(st), right.GetValueType(st));
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            GetValueType(st);
            if(valueType is MapType)
                throw new NotImplementedException(this.Span + " - Operations on maps are not yet implemented.");
            if(valueType is TupleType)
                throw new NotImplementedException(this.Span + " - Operations on tuples are not yet implemented.");
            
            FType targetType = valueType;
            
            if(binOperator is RelationalOperator)
            {
                //we need to cast to the 2same type they would get summed to
                targetType = new PlusOperator(null).GetTarget(left.GetValueType(st), right.GetValueType(st));
            }
            left.Generate(generator, currentType, st, exitLabel, conditionLabel);
            if(FType.SameType(left.GetValueType(st), targetType) == false)
                left.GetValueType(st).ConvertTo(targetType, generator);
            
            right.Generate(generator, currentType, st, exitLabel, conditionLabel);
            if(FType.SameType(right.GetValueType(st),targetType) == false)
                right.GetValueType(st).ConvertTo(targetType, generator);

            string op_name = binOperator.GetMethodName();
            Type rtt = targetType.GetRunTimeType();
            //this goes for binOperator.Generate();
            generator.Emit(OpCodes.Call, rtt.GetMethod(op_name, new Type[]{rtt, rtt}));
        }
    }
    public class NegativeExpression : FExpression
    {
        public FSecondary value;
        public NegativeExpression(FSecondary value, TextSpan span)
        {
            this.Span = span;
            this.value = value;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Negative expression");
            value.Print(tabs + 1);
        }

        public override void BuildType(SymbolTable st)
        {
            valueType = value.GetValueType(st);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            value.Generate(generator, currentType, st, exitLabel, conditionLabel);
            //we call -(obj) for ValueType
            generator.Emit(OpCodes.Call, value.GetValueType(st).GetRunTimeType().GetMethod("op_UnaryNegation", new Type[]{valueType.GetRunTimeType()}));
        }
    }
    public class EllipsisExpression : FExpression
    {
        public FSecondary from;
        public FSecondary to;
        public EllipsisExpression(FSecondary from, FSecondary to, TextSpan span)
        {
            this.Span = span;
            this.from = from;
            this.to = to;
        }
        public override void BuildType(SymbolTable st)
        {
            if(from.GetValueType(st).GetType() != typeof(IntegerType) || to.GetValueType(st).GetType() != typeof(IntegerType))
                throw new NotImplementedException($"{Span} - Can't use ellipsis with {from.GetValueType(st)}-{to.GetValueType(st)}");
            valueType = new EllipsisType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Ellipsis expression");
            from.Print(tabs + 1);
            to.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            from.Generate(generator, currentType, st, exitLabel, conditionLabel);
            to.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Newobj, typeof(FEllipsis).GetConstructor(new Type[]{typeof(FInteger), typeof(FInteger)}));
        }
    }
    public class NotExpression : FExpression
    {
        public FExpression expr;
        public NotExpression(FExpression expr, TextSpan span)
        {
            this.Span = span;
            this.expr = expr;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Not expression");
            expr.Print(tabs + 1);
        }
        public override void BuildType(SymbolTable st)
        {
            valueType = expr.GetValueType(st);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            expr.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Call, expr.GetValueType(st).GetRunTimeType().GetMethod("op_LogicalNot", new Type[]{expr.GetValueType(st).GetRunTimeType()}));
        }
    }

    public class ReadExpression : FExpression
    {
        public FType type;
        public ReadExpression(FType type)
        {
            if(type == null) throw new NotImplementedException($"{Span} - Can't use read keyword without specifying type");
            if(type.GetRunTimeType().GetMethod("Read") == null) throw new NotImplementedException($"{Span} - Read does not support {type}");
            this.type = type;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Read Expression");
            type.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            //Idea is to use runtime function Read(), so that everything depends on library implementation of types
            generator.Emit(OpCodes.Call, type.GetRunTimeType().GetMethod("Read"));
        }
        public override void BuildType(SymbolTable st) => valueType = type;
    }
}