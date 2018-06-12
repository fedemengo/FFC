using System.Collections.Generic;
using System;
using FFC.FParser;

namespace FFC.FAST
{
    class LoopStatement : FStatement
    {
        public FLoopHeader header;
        public StatementList body;

        public LoopStatement(FLoopHeader header, StatementList body, TextSpan span)
        {
            this.Span = span;
            this.header = header;
            this.body = body;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Loop statement");
            header.Print(tabs + 1);
            body.Print(tabs + 1);
        }
    }
    abstract class FLoopHeader : FASTNode
    {
        
    }
    class ForHeader : FLoopHeader
    {
        public Identifier id;
        public FExpression collection;

        public ForHeader(Identifier id, FExpression collection, TextSpan span)
        {
            this.Span = span;
            this.id = id;
            this.collection = collection;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("For header");
            if(id != null) id.Print(tabs + 1);
            collection.Print(tabs + 1);
        }
    }
    class WhileHeader : FLoopHeader
    {
        public FExpression condition;

        public WhileHeader(FExpression condition, TextSpan span)
        {
            this.Span = span;
            this.condition = condition;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("While header");
            condition.Print(tabs + 1);
        }
    }
}