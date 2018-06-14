using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using FFC.FParser;

namespace FFC.FAST
{
    abstract class FExpression : FASTNode
    {
        /*
            inherited by
                BinOpExpr
                NegExpr
                EllipsisExpr
                FSecondary
        */
        public virtual void EmitPrint(ILGenerator generator)
        {
            Generate(generator);
            generator.Emit(OpCodes.Call, typeof(System.Console).GetMethod("Write", new Type[]{ValueType.GetRunTimeType()}));
        }
        private FType _type;
        public virtual FType ValueType
        {
            set => _type = value;
            get
            {
                if(_type == null)
                    BuildType();
                return _type;
            }
        }
        public virtual void BuildType()
        {
            throw new NotImplementedException();
        }
    }
    class ExpressionList : FASTNode
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
    class BinaryOperatorExpression : FExpression
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
        public override void BuildType()
        {
            ValueType = binOperator.GetTarget(left.ValueType, right.ValueType);
        }

        public override void Generate(ILGenerator generator)
        {
            if(ValueType is ComplexType || ValueType is RationalType)
                throw new NotImplementedException(this.Span + " - Operations on rational / complex are not yet implemented.");
            if(ValueType is ArrayType)
                throw new NotImplementedException(this.Span + " - Operations on arrays are not yet implemented.");
            if(ValueType is MapType)
                throw new NotImplementedException(this.Span + " - Operations on maps are not yet implemented.");
            if(ValueType is TupleType)
                throw new NotImplementedException(this.Span + " - Operations on tuples are not yet implemented.");
            
            FType targetType = ValueType;
            
            if(binOperator is RelationalOperator)
            {
                //we need to cast to the same type they would get summed to
                targetType = new PlusOperator(null).GetTarget(left.ValueType, right.ValueType);
            }

            left.Generate(generator);
            if(left.ValueType.GetType() != targetType.GetType())
                FType.Convert(left.ValueType, targetType, generator);
            
            right.Generate(generator);
            if(right.ValueType.GetType() != targetType.GetType())
                FType.Convert(right.ValueType, targetType, generator);

            string op_name = binOperator.GetMethodName();
            Type rtt = targetType.GetRunTimeType();
            //this goes for binOperator.Generate();
            generator.Emit(OpCodes.Call, rtt.GetMethod(op_name, new Type[]{rtt, rtt}));
        }

        public override void EmitPrint(ILGenerator generator)
        {
            Generate(generator);
            generator.Emit(OpCodes.Call, typeof(System.Console).GetMethod("Write", new Type[]{ValueType.GetRunTimeType()}));
        }
    }
    class NegativeExpression : FExpression
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

        public override void BuildType()
        {
            ValueType = value.ValueType;
        }
        public override void Generate(ILGenerator generator)
        {
            value.Generate(generator);
            generator.Emit(OpCodes.Neg);
        }
    }
    class EllipsisExpression : FExpression
    {
        public FSecondary from;
        public FSecondary to;
        public EllipsisExpression(FSecondary from, FSecondary to, TextSpan span)
        {
            this.Span = span;
            this.from = from;
            this.to = to;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Ellipsis expression");
            from.Print(tabs + 1);
            to.Print(tabs + 1);
        }
    }
    class NotExpression : FExpression
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
        public override void BuildType()
        {
            ValueType = expr.ValueType;
        }
        public override void Generate(ILGenerator generator)
        {
            // "!a" -> "a = 0"
            expr.Generate(generator);
            if(expr.ValueType is BooleanType == false)
            {
                throw new Exception($"Can't apply not operator to {expr.ValueType}");
            }
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ceq);
        }
    }
}