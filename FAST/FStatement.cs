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
    }
    class ExpressionStatement : FStatement
    {
        public FExpression expression;
        public ExpressionStatement(FExpression expression)
        {
            this.expression = expression;
        }
        
    }
    class FunctionCallStatement : FStatement
    {
        public FunctionCall function;
        public FunctionCallStatement(FunctionCall function)
        {
            this.function = function;
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
    }
    class DeclarationStatementList : FASTNode
    {
        public List<DeclarationStatement> statements;
        public DeclarationStatementList(DeclarationStatement stm)
        {
            statements = new List<DeclarationStatement>{stm};
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
    }
    class BreakStatement : FStatement
    {
        public BreakStatement()
        {

        }
    }
    class ContinueStatement : FStatement
    {
        public ContinueStatement()
        {

        }
    }
    class PrintStatement : FStatement
    {
        public ExpressionList toPrint;
        public PrintStatement(ExpressionList toPrint)
        {
            this.toPrint = toPrint;
        }
    }
}