using System.Collections.Generic;
using System;
using System.Reflection.Emit;
using FFC.FParser;
using FFC.FGen;

namespace FFC.FAST
{
    public abstract class FOperator : FASTNode
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
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            throw new NotImplementedException($"{this.Span} - Operators are not meant to be directly generated.");
        }
        public virtual string GetMethodName()
        {
            throw new NotImplementedException($"{this.Span} - GetMethodName not implemented for {this.GetType().Name}");
        }
    }

    public abstract class RelationalOperator : FOperator
    {
        public override FType GetTarget(FType t1, FType t2)
        {
            //to handle recursion
            if(t1 == null || t2 == null) return null;

            if(!(t1 is NumericType && t2 is NumericType))
                throw new NotImplementedException($"{this.Span.Begin} - Can't compare non numeric types");
            if(t1 is ComplexType || t2 is ComplexType)
            {
                if(!(t1 is ComplexType && t2 is ComplexType))
                    throw new NotImplementedException($"{this.Span.Begin} - Can't compare complex numbers to other numeric values");
            }
            if(t1 is RationalType || t2 is RationalType){
                if(t1 is RealType || t2 is RealType)
                    throw new NotImplementedException($"{this.Span.Begin} - Can't compare rationals to reals");
            }
            return new BooleanType();
        }
    }
    public class LessOperator : RelationalOperator
    {
        public LessOperator(TextSpan span)
        {
            Span = span;
        }
        public override string GetMethodName() => "op_LessThan";
    }
    public class LessEqualOperator : RelationalOperator
    {
        public LessEqualOperator(TextSpan span)
        {
            Span = span;
        }
        public override string GetMethodName() => "op_LessThanOrEqual";
    }
    public class GreaterOperator : RelationalOperator
    {
        public GreaterOperator(TextSpan span)
        {
            Span = span;
        }
        public override string GetMethodName() => "op_GreaterThan";
    }
    public class GreaterEqualOperator : RelationalOperator
    {
        public GreaterEqualOperator(TextSpan span)
        {
            Span = span;
        }
        public override string GetMethodName() => "op_GreaterThanOrEqual";
    }
    public class EqualOperator : RelationalOperator
    {
        public EqualOperator(TextSpan span)
        {
            Span = span;
        }
        public override string GetMethodName() => "op_Equality";
    }
    public class NotEqualOperator : RelationalOperator
    {
        public NotEqualOperator(TextSpan span)
        {
            Span = span;
        }

        public override string GetMethodName() => "op_Inequality";
    }

    public abstract class BooleanOperator : FOperator
    {
        public override FType GetTarget(FType t1, FType t2)
        {
            //to handle recursion
            if(t1 == null || t2 == null) return null;

            if(t1 is BooleanType && t2 is BooleanType)
                return new BooleanType();
            throw new NotImplementedException($"{this.Span.Begin} - Can't use boolean operator {this.GetType().Name} on non-boolean values");            
        }
    }
    public class AndOperator : BooleanOperator
    {
        public AndOperator(TextSpan span)
        {
            Span = span;
        }
        public override string GetMethodName() => "op_BitwiseAnd";
    }
    public class OrOperator : BooleanOperator
    {
        public OrOperator(TextSpan span)
        {
            Span = span;
        }
        public override string GetMethodName() => "op_BitwiseOr";
    }
    public class XorOperator : BooleanOperator
    {
        public XorOperator(TextSpan span)
        {
            Span = span;
        }
        public override string GetMethodName() => "op_ExclusiveOr";
    }

    public abstract class MathOperator : FOperator
    {
        public override FType GetTarget(FType t1, FType t2)
        {
            //to handle recursion
            if(t1 == null || t2 == null) return null;

            if(!(t1 is NumericType && t2 is NumericType))
                throw new NotImplementedException($"{this.Span.Begin} - Can't apply operator {this.GetType().Name} to non-numeric type {(t1 is NumericType ? t2.GetType().Name : t1.GetType().Name)}");
            if(t1 is ComplexType || t2 is ComplexType)
            {
                if(t1 is RationalType || t2 is RationalType)
                    throw new NotImplementedException($"{this.Span.Begin} - Can't use operator {this.GetType().Name} mixing complex and rational numbers");
                return new ComplexType();
            }
            if(t1 is RationalType || t2 is RationalType)
            {
                if(t1 is RealType || t2 is RealType)
                    throw new NotImplementedException($"{this.Span.Begin} - Can't use operator {this.GetType().Name} mixing real and rational numbers");
                return new RationalType();
            }
            if(t1 is RealType || t2 is RealType)
                return new RealType();
            return new IntegerType();
        }
    }
    public class PlusOperator : MathOperator
    {
        public PlusOperator(TextSpan span)
        {
            Span = span;
        }
        public override string GetMethodName() => "op_Addition";

        public override FType GetTarget(FType t1, FType t2)
        {
            //to handle recursion
            if(t1 == null || t2 == null) return null;

            if(t1 is StringType && t2 is StringType)
                return new StringType();
            if(t1 is ArrayType && t2 is ArrayType && ((ArrayType) t1).Type.GetType() == ((ArrayType) t2).Type.GetType())
                return new ArrayType(((ArrayType) t1).Type);
            if(t1 is ArrayType && t2.GetType() == ((ArrayType) t1).Type.GetType())
                return new ArrayType(((ArrayType) t1).Type);
            return base.GetTarget(t1, t2);
        }
    }
    public class MinusOperator : MathOperator
    {
        public MinusOperator(TextSpan span)
        {
            Span = span;
        }

        public override string GetMethodName() => "op_Subtraction";
    }
    public class StarOperator : MathOperator
    {
        public StarOperator(TextSpan span)
        {
            Span = span;
        }

        public override string GetMethodName() => "op_Multiply";
    }
    public class SlashOperator : MathOperator
    {
        public SlashOperator(TextSpan span)
        {
            Span = span;
        }

        public override string GetMethodName() => "op_Division";

        public override FType GetTarget(FType t1, FType t2)
        {
            //to handle recursion
            if(t1 == null || t2 == null) return null;

            if(!(t1 is NumericType && t2 is NumericType))
                throw new NotImplementedException($"{this.Span.Begin} - Can't use {this.GetType().Name} with non-numeric type {(t1 is NumericType ? t2.GetType().Name : t1.GetType().Name)}");
            if(t1 is ComplexType || t2 is ComplexType)
            {
                if(t1 is RationalType || t2 is RationalType)
                    throw new NotImplementedException($"{this.Span.Begin} - Can't use {this.GetType().Name} mixing complex and rational values");
                return new ComplexType();
            }
            if(t1 is RationalType || t2 is RationalType)
            {
                if(t1 is RealType || t2 is RealType)
                    throw new NotImplementedException($"{this.Span.Begin} - Can't use {this.GetType().Name} mixing real and rational values");
                return new RationalType();
            }
            return new RealType();
        }
    }
    public class ModuloOperator : MathOperator
    {
        public ModuloOperator(TextSpan span)
        {
            Span = span;
        }
        public override string GetMethodName() => "op_Modulus";
        public override FType GetTarget(FType t1, FType t2)
        {
            //to handle recursion
            if(t1 == null || t2 == null) return null;

            if(t1 is IntegerType && t2 is IntegerType)
                return new IntegerType();
            throw new NotImplementedException($"{this.Span.Begin} - Can't use {this.GetType().Name} with {(t1 is IntegerType ? t2.GetType().Name : t1.GetType().Name)} values");
        }
    }
}