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
    class DeclarationStatementList
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
        public List<FStatement> ifTrue;
        public List<FStatement> ifFalse;
        public IfStatement(FExpression condition, List<FStatement> ifTrue, List<FStatement> ifFalse)
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
        List<FExpression> toPrint;
        public PrintStatement(List<FExpression> toPrint)
        {
            this.toPrint = toPrint;
        }
    }
}