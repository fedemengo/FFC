using System.Collections.Generic;
using System;
using System.Reflection.Emit;
using FFC.FParser;

namespace FFC.FAST
{
    abstract class FOperator : FASTNode
    {
        /*
            inherited by
            LessOperator
            LessEqualOperator
            GreaterOperator
            GreaterEqualOperator
            EqualOperator
            NotEqualOperator
            AndOperator
            OrOperator
            XorOperator
            PlusOperator
            MinusOperator
            StarOperator
            SlashOperator
        */
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(this.GetType().ToString().Substring(9));
            Console.ForegroundColor = prev;
        }
        public abstract FType GetTarget(FType t1, FType t2);
        public override void Generate(ILGenerator generator)
        {
            throw new NotImplementedException($"{this.Span} - Operators are not meant to be directly generated.");
        }
        public virtual string GetMethodName()
        {
            throw new NotImplementedException($"{this.Span} - GetMethodName not implemented for {this.GetType().Name}");
        }
    }

    abstract class RelationalOperator : FOperator
    {
        public override FType GetTarget(FType t1, FType t2)
        {
            if(!(t1 is NumericType && t2 is NumericType))
                throw new Exception($"{this.Span.Begin} - Can't compare non numeric types");
            if(t1 is ComplexType || t2 is ComplexType)
            {
                if(!(t1 is ComplexType && t2 is ComplexType))
                    throw new Exception($"{this.Span.Begin} - Can't compare complex numbers to other numeric values");
            }
            if(t1 is RationalType || t2 is RationalType){
                if(t1 is RealType || t2 is RealType)
                    throw new Exception($"{this.Span.Begin} - Can't compare rationals to reals");
            }
            return new BooleanType();
        }
    }
    class LessOperator : RelationalOperator
    {
        public LessOperator(TextSpan span)
        {
            this.Span = span;
        }
        public override string GetMethodName() => "op_LessThan";
    }
    class LessEqualOperator : RelationalOperator
    {
        public LessEqualOperator(TextSpan span)
        {
            this.Span = span;
        }
        public override string GetMethodName() => "op_LessThanOrEqual";
    }
    class GreaterOperator : RelationalOperator
    {
        public GreaterOperator(TextSpan span)
        {
            this.Span = span;
        }
        public override string GetMethodName() => "op_GreaterThan";
    }
    class GreaterEqualOperator : RelationalOperator
    {
        public GreaterEqualOperator(TextSpan span)
        {
            this.Span = span;
        }
        public override string GetMethodName() => "op_GreaterThanOrEqual";
    }
    class EqualOperator : RelationalOperator
    {
        public EqualOperator(TextSpan span)
        {
            this.Span = span;
        }
        public override string GetMethodName() => "op_Equality";
    }
    class NotEqualOperator : RelationalOperator
    {
        public NotEqualOperator(TextSpan span)
        {
            this.Span = span;
        }

        public override string GetMethodName() => "op_Inequality";
    }

    abstract class BooleanOperator : FOperator
    {
        public override FType GetTarget(FType t1, FType t2)
        {
            if(t1 is BooleanType && t2 is BooleanType)
                return new BooleanType();
            throw new Exception($"{this.Span.Begin} - Can't use boolean operator {this.GetType().Name} on non-boolean values");            
        }
    }
    class AndOperator : BooleanOperator
    {
        public AndOperator(TextSpan span)
        {
            this.Span = span;
        }
        public override string GetMethodName() => "op_BitwiseAnd";
    }
    class OrOperator : BooleanOperator
    {
        public OrOperator(TextSpan span)
        {
            this.Span = span;
        }
        public override string GetMethodName() => "op_BitwiseOr";
    }
    class XorOperator : BooleanOperator
    {
        public XorOperator(TextSpan span)
        {
            this.Span = span;
        }
        public override string GetMethodName() => "op_ExclusiveOr";
    }

    abstract class MathOperator : FOperator
    {
        public override FType GetTarget(FType t1, FType t2)
        {
            if(!(t1 is NumericType && t2 is NumericType))
                throw new Exception($"{this.Span.Begin} - Can't apply operator {this.GetType().Name} to non-numeric type {(t1 is NumericType ? t2.GetType().Name : t1.GetType().Name)}");
            if(t1 is ComplexType || t2 is ComplexType)
            {
                if(t1 is RationalType || t2 is RationalType)
                    throw new Exception($"{this.Span.Begin} - Can't use operator {this.GetType().Name} mixing complex and rational numbers");
                return new ComplexType();
            }
            if(t1 is RationalType || t2 is RationalType)
            {
                if(t1 is RealType || t2 is RealType)
                    throw new Exception($"{this.Span.Begin} - Can't use operator {this.GetType().Name} mixing real and rational numbers");
                return new RationalType();
            }
            if(t1 is RealType || t2 is RealType)
                return new RealType();
            return new IntegerType();
        }
    }
    class PlusOperator : MathOperator
    {
        public PlusOperator(TextSpan span)
        {
            this.Span = span;
        }

        public override string GetMethodName() => "op_Addition";

        public override FType GetTarget(FType t1, FType t2)
        {
            if(t1 is StringType && t2 is StringType)
                return new StringType();
            if(t1 is ArrayType && t2 is ArrayType && ((ArrayType) t1).type.GetType() == ((ArrayType) t2).type.GetType())
                return new ArrayType(((ArrayType) t1).type);
            if(t1 is ArrayType && t2.GetType() == ((ArrayType) t1).type.GetType())
                return new ArrayType(((ArrayType) t1).type);
            return base.GetTarget(t1, t2);
        }
    }
    class MinusOperator : MathOperator
    {
        public MinusOperator(TextSpan span)
        {
            this.Span = span;
        }

        public override string GetMethodName() => "op_Subtraction";
    }
    class StarOperator : MathOperator
    {
        public StarOperator(TextSpan span)
        {
            this.Span = span;
        }

        public override string GetMethodName() => "op_Multiply";
    }
    class SlashOperator : MathOperator
    {
        public SlashOperator(TextSpan span)
        {
            this.Span = span;
        }

        public override string GetMethodName() => "op_Division";

        public override FType GetTarget(FType t1, FType t2)
        {
            if(!(t1 is NumericType && t2 is NumericType))
                throw new Exception($"{this.Span.Begin} - Can't use {this.GetType().Name} with non-numeric type {(t1 is NumericType ? t2.GetType().Name : t1.GetType().Name)}");
            if(t1 is ComplexType || t2 is ComplexType)
            {
                if(t1 is RationalType || t2 is RationalType)
                    throw new Exception($"{this.Span.Begin} - Can't use {this.GetType().Name} mixing complex and rational values");
                return new ComplexType();
            }
            if(t1 is RationalType || t2 is RationalType)
            {
                if(t1 is RealType || t2 is RealType)
                    throw new Exception($"{this.Span.Begin} - Can't use {this.GetType().Name} mixing real and rational values");
                return new RationalType();
            }
            return new RealType();
        }
    }
    class ModuloOperator : MathOperator
    {
        public ModuloOperator(TextSpan span)
        {
            this.Span = span;
        }
        public override string GetMethodName() => "op_Modulus";
        public override FType GetTarget(FType t1, FType t2)
        {
            if(t1 is IntegerType && t2 is IntegerType)
                return new IntegerType();
            throw new Exception($"{this.Span.Begin} - Can't use {this.GetType().Name} with {(t1 is IntegerType ? t2.GetType().Name : t1.GetType().Name)} values");
        }
    }
}