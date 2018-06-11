using System.Collections.Generic;
using System;
using System.Reflection.Emit;

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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(this.GetType().ToString().Substring(9));
            Console.ForegroundColor = prev;
        }
        public abstract FType GetTarget(FType t1, FType t2);
    }

    abstract class RelationalOperator : FOperator
    {
        public override FType GetTarget(FType t1, FType t2)
        {
            if(!(t1 is NumericType && t2 is NumericType))
                throw new Exception("Can't get target type");
            if(t1 is ComplexType || t2 is ComplexType)
            {
                if(!(t1 is ComplexType && t2 is ComplexType))
                    throw new Exception("Can't get target type");
            }
            if(t1 is RationalType || t2 is RationalType){
                if(t1 is RealType || t2 is RealType)
                    throw new Exception("Can't get target type");
            }
            return new BooleanType();
        }
    }
    class LessOperator : RelationalOperator
    {
        public LessOperator()
        {

        }
        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Clt);
        }
    }
    class LessEqualOperator : RelationalOperator
    {
        public LessEqualOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Cgt);            // <= equivalent to !>
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ceq);
        }
    }
    class GreaterOperator : RelationalOperator
    {
        public GreaterOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Cgt);
        }
    }
    class GreaterEqualOperator : RelationalOperator
    {
        public GreaterEqualOperator()
        {

        }
        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Clt);            // >= equivalent to !<
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ceq);
        }
    }
    class EqualOperator : RelationalOperator
    {
        public EqualOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Ceq);
        }
    }
    class NotEqualOperator : RelationalOperator
    {
        public NotEqualOperator()
        {
        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Ceq);
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Ceq);
        }

    }

    abstract class BooleanOperator : FOperator
    {
        public override FType GetTarget(FType t1, FType t2)
        {
            if(t1 is BooleanType && t2 is BooleanType)
                return new BooleanType();
            throw new Exception("Can't get target type");            
        }
    }
    class AndOperator : BooleanOperator
    {
        public AndOperator()
        {

        }
        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.And);
        }
    }
    class OrOperator : BooleanOperator
    {
        public OrOperator()
        {

        }
        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Or);
        }
    }
    class XorOperator : BooleanOperator
    {
        public XorOperator()
        {

        }
        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Xor);
        }
    }

    abstract class MathOperator : FOperator
    {
        public override FType GetTarget(FType t1, FType t2)
        {
            if(!(t1 is NumericType && t2 is NumericType))
                throw new Exception("Can't get target type");
            if(t1 is ComplexType || t2 is ComplexType)
            {
                if(t1 is RationalType || t2 is RationalType)
                    throw new Exception("Can't get target type");
                return new ComplexType();
            }
            if(t1 is RationalType || t2 is RationalType)
            {
                if(t1 is RealType || t2 is RealType)
                    throw new Exception("Can't get target type");
                return new RationalType();
            }
            if(t1 is RealType || t2 is RealType)
                return new RealType();
            return new IntegerType();
        }
    }
    class PlusOperator : MathOperator
    {
        public PlusOperator()
        {
        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Add);
        }

        public override FType GetTarget(FType t1, FType t2)
        {
            if(t1 is StringType && t2 is StringType)
                return new StringType();
            if(t1 is ArrayType && t2 is ArrayType && ((ArrayType) t1).type.GetType() == ((ArrayType) t2).type.GetType())
                return new ArrayType(((ArrayType) t1).type);
            if(t1 is ArrayType && t2.GetType() == ((ArrayType) t1).type.GetType())
                return new ArrayType(((ArrayType) t1).type);
            return (this as MathOperator).GetTarget(t1, t2);
        }
    }
    class MinusOperator : MathOperator
    {
        public MinusOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Sub);
        }
    }
    class StarOperator : MathOperator
    {
        public StarOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Mul);
        }
    }
    class SlashOperator : MathOperator
    {
        public SlashOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Div);
        }

        public override FType GetTarget(FType t1, FType t2)
        {
            if(!(t1 is NumericType && t2 is NumericType))
                throw new Exception("Can't get target type");
            if(t1 is ComplexType || t2 is ComplexType)
            {
                if(t1 is RationalType || t2 is RationalType)
                    throw new Exception("Can't get target type");
                return new ComplexType();
            }
            if(t1 is RationalType || t2 is RationalType)
            {
                if(t1 is RealType || t2 is RealType)
                    throw new Exception("Can't get target type");
                return new RationalType();
            }
            return new RealType();
        }
    }
    class ModuloOperator : MathOperator
    {
        public ModuloOperator()
        {
            
        }
        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Rem);
        }

        public override FType GetTarget(FType t1, FType t2)
        {
            if(t1 is IntegerType && t2 is IntegerType)
                return new IntegerType();
            throw new Exception("Can't get target type");
        }
    }
}