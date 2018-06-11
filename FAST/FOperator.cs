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

    }
    class LessOperator : FOperator
    {
        public LessOperator()
        {

        }
        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Clt);
        }
    }
    class LessEqualOperator : FOperator
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
    class GreaterOperator : FOperator
    {
        public GreaterOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Cgt);
        }
    }
    class GreaterEqualOperator : FOperator
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
    class EqualOperator : FOperator
    {
        public EqualOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Ceq);
        }
    }
    class NotEqualOperator : FOperator
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
    class AndOperator : FOperator
    {
        public AndOperator()
        {

        }
        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.And);
        }
    }
    class OrOperator : FOperator
    {
        public OrOperator()
        {

        }
        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Or);
        }
    }
    class XorOperator : FOperator
    {
        public XorOperator()
        {

        }
        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Xor);
        }
    }
    class PlusOperator : FOperator
    {
        public PlusOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Add);
        }
    }
    class MinusOperator : FOperator
    {
        public MinusOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Sub);
        }
    }
    class StarOperator : FOperator
    {
        public StarOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Mul);
        }
    }
    class SlashOperator : FOperator
    {
        public SlashOperator()
        {

        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Div);
        }
    }
    class ModuloOperator : FOperator
    {
        public ModuloOperator()
        {
            
        }
        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Rem);
        }
    }
}