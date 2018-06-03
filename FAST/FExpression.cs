using System;
using System.Collections.Generic;

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
    }
    class ExpressionList : FASTNode
    {
        public List<FExpression> expressions;
        public ExpressionList(FExpression expr)
        {
            expressions = new List<FExpression>{expr};
        }
        public ExpressionList()
        {
            expressions = new List<FExpression>();
        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Expression list");
            foreach(FExpression e in expressions)
                e.Print(tabs + 1);
        }

    }
    class BinaryOperatorExpression : FExpression
    {
        public FSecondary left;
        public FOperator binOperator;
        public FExpression right;
        public BinaryOperatorExpression(FSecondary left, FOperator binOperator, FExpression right)
        {
            this.left = left;
            this.binOperator = binOperator;
            this.right = right;
        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Binary operator");
            left.Print(tabs);
            binOperator.Print(tabs);
            right.Print(tabs);
        }
        
    }
    class NegativeExpression : FExpression
    {
        public FSecondary value;
        public NegativeExpression(FSecondary value)
        {
            this.value = value;
        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Negative expression");
            value.Print(tabs + 1);
        }

    }
    class EllipsisExpression : FExpression
    {
        public FSecondary from;
        public FSecondary to;
        public EllipsisExpression(FSecondary from, FSecondary to)
        {
            this.from = from;
            this.to = to;
        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Ellipsis expression");
            from.Print(tabs + 1);
            to.Print(tabs + 1);
        }

    }
}