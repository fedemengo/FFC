using System;
using System.Collections.Generic;

namespace FFC.FAST
{
    abstract class FStatement : FASTNode
    {
        /*
            inherited by
            FunctionCallStatement
            AssignmentStatemt
            DeclStm
            IfStm
            LoopStm
            ReturnStm
            BreakStm
            ContinueStm
            PrintStm
         */
    }
    class StatementList : FASTNode
    {
        public List<FStatement> statements;
        public StatementList(FStatement statement)
        {
            statements = new List<FStatement>{statement};
        }
        public StatementList()
        {
            statements = new List<FStatement>();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Statement list");
            foreach(FStatement fs in statements)
                fs.Print(tabs + 1);
        }
    }
    class ExpressionStatement : FStatement
    {
        public FExpression expression;
        public ExpressionStatement(FExpression expression)
        {
            this.expression = expression;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Expression statement");
            expression.Print(tabs + 1);
        }

        
    }
    class FunctionCallStatement : FStatement
    {
        public FunctionCall function;
        public FunctionCallStatement(FunctionCall function)
        {
            this.function = function;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("FunctionCall statement");
            function.Print(tabs + 1);
        }
    }
    class AssignmentStatemt : FStatement
    {
        public FSecondary left;
        public FExpression right;
        public AssignmentStatemt(FSecondary left, FExpression right)
        {
            this.left = left;
            this.right = right;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Asssignement statement");
            left.Print(tabs + 1);
            right.Print(tabs + 1);
        }
    }
    class DeclarationStatement : FStatement
    {
        public Identifier id;
        public FType type;
        public FExpression expr;
        public DeclarationStatement(Identifier id, FType type, FExpression expr)
        {
            this.id = id;
            this.type = type;
            this.expr = expr;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Declaration statement");
            id.Print(tabs + 1);
            if(type != null) type.Print(tabs + 1);
            expr.Print(tabs + 1);
        }
    }
    class DeclarationStatementList : FASTNode
    {
        public List<DeclarationStatement> statements;
        public DeclarationStatementList(DeclarationStatement stm)
        {
            statements = new List<DeclarationStatement>{stm};
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Declaration statement list");
            foreach(DeclarationStatement stm in statements)
                stm.Print(tabs + 1);
        }
    }
    class IfStatement : FStatement
    {
        public FExpression condition;
        public StatementList ifTrue;
        public StatementList ifFalse;
        public IfStatement(FExpression condition, StatementList ifTrue, StatementList ifFalse)
        {
            this.condition = condition;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("If statement");
            condition.Print(tabs + 1);
            ifTrue.Print(tabs + 1);
            ifFalse.Print(tabs + 1);
        }
    }
    class ReturnStatement : FStatement
    {
        public FExpression value;
        public ReturnStatement(FExpression value)
        {
            this.value = value;
        }
        public ReturnStatement()
        {
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Return statement");
            if(value != null)
                value.Print(tabs + 1);
        }
    }
    class BreakStatement : FStatement
    {
        public BreakStatement()
        {

        }public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Break statement");
        }

    }
    class ContinueStatement : FStatement
    {
        public ContinueStatement()
        {

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Continue statement");
        }
    }
    class PrintStatement : FStatement
    {
        public ExpressionList toPrint;
        public PrintStatement(ExpressionList toPrint)
        {
            this.toPrint = toPrint;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Print statement");
            toPrint.Print(tabs + 1);
        }
    }
}