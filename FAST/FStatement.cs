using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using FFC.FParser;
using FFC.FRunTime;
using FFC.FGen;

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
        public StatementList(FStatement statement, TextSpan span)
        {
            this.Span = span;
            statements = new List<FStatement>{statement};
        }
        public StatementList(TextSpan span)
        {
            this.Span = span;
            statements = new List<FStatement>();
        }
        public void Add(FStatement stm)
        {
            statements.Add(stm);
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Statement list");
            foreach(FStatement fs in statements)
                fs.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            foreach(FStatement stm in statements)
                if(stm is DeclarationStatement)
                    (stm as DeclarationStatement).Generate(generator, ref st);
                else
                    stm.Generate(generator, st);
        }
        public void Generate(ILGenerator generator, Label conditionLabel, Label exitLabel, SymbolTable st)
        {
            foreach(FStatement stm in statements)
            {
                if(stm is BreakStatement) (stm as BreakStatement).Generate(generator, exitLabel);
                else if (stm is ContinueStatement) (stm as ContinueStatement).Generate(generator, conditionLabel);
                else stm.Generate(generator, st);
            }
        }
    }
    class ExpressionStatement : FStatement
    {
        public FExpression expression;
        public ExpressionStatement(FExpression expression, TextSpan span)
        {
            this.Span = span;
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
        public FunctionCallStatement(FunctionCall function, TextSpan span)
        {
            this.Span = span;
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
        public AssignmentStatemt(FSecondary left, FExpression right, TextSpan span)
        {
            this.Span = span;
            this.left = left;
            this.right = right;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Assignement statement");
            left.Print(tabs + 1);
            right.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            if(left is Identifier)
            {
                Identifier x = left as Identifier;
                var y = st.Find(x.name);
                if(y == null) throw new NotImplementedException($"{Span} - Identifier {(left as Identifier).name} is not declared");
                if(right.GetValueType(st).GetRunTimeType() != y.Type.GetRunTimeType()) throw new NotImplementedException($"{Span} - Can't assign type {right.GetValueType(st).GetRunTimeType()} to variable of type {x.GetValueType(st).GetRunTimeType()}"); 
                right.Generate(generator, st);
                generator.Emit(OpCodes.Stloc, y.LocBuilder);
            }
            else if(left is IndexedAccess)
            {
                IndexedAccess x = left as IndexedAccess;
                x.container.Generate(generator, st);
                x.index.Generate(generator, st);
                //generate expression to load
                right.Generate(generator, st);
                //ckeck types - currently too sleepy!
                if(x.index is SquaresIndexer)
                    generator.Emit(OpCodes.Callvirt, x.container.GetValueType(st).GetRunTimeType().GetMethod("set_Item", new Type[]{x.index.GetValueType(st).GetRunTimeType(), right.GetValueType(st).GetRunTimeType()}));
                else
                    throw new NotImplementedException($"{Span} - Generation not supported for {x.index.GetType().Name}");
            }
            else throw new NotImplementedException($"{Span} - Assignments to {left.GetType().Name} are not implemented");
        }
    }
    class DeclarationStatement : FStatement
    {
        public Identifier id;
        public FType type;
        public FExpression expr;
        public DeclarationStatement(Identifier id, FType type, FExpression expr, TextSpan span)
        {
            this.Span = span;
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

        public void Generate(ILGenerator generator, ref SymbolTable st)
        {
            FType t = expr.GetValueType(st);
            if(type != null && type.GetType() != t.GetType())
                throw new NotImplementedException($"{Span} - Type doesn't match declaration");
            LocalBuilder var = generator.DeclareLocal(t.GetRunTimeType());
            st = (SymbolTable) st.Assign(id.name, new Data(var, t));
            expr.Generate(generator, st);
            generator.Emit(OpCodes.Stloc, var);
        }
    }
    class DeclarationStatementList : FASTNode
    {
        public List<DeclarationStatement> statements;
        public DeclarationStatementList(DeclarationStatement stm, TextSpan span)
        {
            this.Span = span;
            statements = new List<DeclarationStatement>{stm};
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Declaration statement list");
            foreach(DeclarationStatement stm in statements)
                stm.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            if(statements.Count == 1)
                ((statements[0]).expr as FunctionDefinition).body.Generate(generator, st);
            else
                foreach(var stm in statements)
                    stm.Generate(generator, ref st);
        }
        public void Add(DeclarationStatement stm)
        {
            statements.Add(stm);
        }
    }
    class IfStatement : FStatement
    {
        public FExpression condition;
        public StatementList ifTrue;
        public ElseIfList elseIfs;
        public StatementList ifFalse;
        public IfStatement(FExpression condition, StatementList ifTrue, ElseIfList elseIfs, StatementList ifFalse, TextSpan span)
        {
            this.Span = span;
            this.condition = condition;
            this.ifTrue = ifTrue;
            this.elseIfs = elseIfs;
            this.ifFalse = ifFalse;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("If statement");
            condition.Print(tabs + 1);
            ifTrue.Print(tabs + 1);
            elseIfs.Print(tabs + 1);
            ifFalse.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            if(condition.GetValueType(st).GetRunTimeType() != typeof(FBoolean))
            {
                throw new NotImplementedException($"{Span} - Can't use conditional with {condition.GetValueType(st)}");
            }
            condition.Generate(generator, st);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
            
            Label falseBranch = generator.DefineLabel();
            Label exitBranch = generator.DefineLabel();

            generator.Emit(OpCodes.Brfalse, falseBranch);
            ifTrue.Generate(generator, st);
            generator.Emit(OpCodes.Br, exitBranch);
            generator.MarkLabel(falseBranch);
            elseIfs.Generate(generator, exitBranch, st);
            ifFalse.Generate(generator, st);
            generator.MarkLabel(exitBranch);
        }
    }

    class ElseIfList : FASTNode
    {
        public List<ElseIfStatement> list;
        public void Add(ElseIfStatement other)
        {
            list.Add(other);
        }

        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Else if list");
            foreach(var l in list)
                l.Print(tabs + 1);
        }

        public ElseIfList(TextSpan span = null)
        {
            this.Span = span;
            list = new List<ElseIfStatement>();
        }
        public ElseIfList(ElseIfStatement start, TextSpan span)
        {
            this.Span = span;
            list = new List<ElseIfStatement>{start};
        }

        public void Generate(ILGenerator generator, Label exitBranch, SymbolTable st)
        {
            foreach(ElseIfStatement elif in list)
                elif.Generate(generator, exitBranch, st);
        }
    }

    class ElseIfStatement : FASTNode
    {
        public FExpression condition;
        public StatementList ifTrue;
        public ElseIfStatement(FExpression condition, StatementList ifTrue, TextSpan span)
        {
            this.Span = span;
            this.condition = condition;
            this.ifTrue = ifTrue;
        }

        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Else if");
            condition.Print(tabs + 1);
            ifTrue.Print(tabs + 1);
        }

        public void Generate(ILGenerator generator, Label exitBranch, SymbolTable st)
        {
            if(condition.GetValueType(st).GetRunTimeType() != typeof(FBoolean))
            {
                throw new NotImplementedException($"{Span} - Can't use conditional with {condition.GetValueType(st)}");
            }
            condition.Generate(generator, st);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
            Label falseBranch = generator.DefineLabel();

            generator.Emit(OpCodes.Brfalse, falseBranch);
            ifTrue.Generate(generator, st);
            generator.Emit(OpCodes.Br, exitBranch);
            generator.MarkLabel(falseBranch);
        }
    }

    class ReturnStatement : FStatement
    {
        public FExpression value = null;
        public ReturnStatement(FExpression value, TextSpan span)
        {
            this.Span = span;
            this.value = value;
        }
        public ReturnStatement(TextSpan span)
        {
            this.Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Return statement");
            if(value != null)
                value.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            if(value != null) value.Generate(generator, st);
            generator.Emit(OpCodes.Ret);
        }
    }
    class BreakStatement : FStatement
    {
        public BreakStatement(TextSpan span)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Break statement");
        }

        public void Generate(ILGenerator generator, Label exitLabel)
        {
            generator.Emit(OpCodes.Br, exitLabel);
        }
    }
    class ContinueStatement : FStatement
    {
        public ContinueStatement(TextSpan span)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Continue statement");
        }

        public void Generate(ILGenerator generator, Label conditionLabel)
        {
            generator.Emit(OpCodes.Br, conditionLabel);
        }
    }
    class PrintStatement : FStatement
    {
        public ExpressionList toPrint;
        public PrintStatement(ExpressionList toPrint, TextSpan span)
        {
            this.Span = span;
            this.toPrint = toPrint;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Print statement");
            toPrint.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            foreach(FExpression expr in toPrint.expressions){
                expr.EmitPrint(generator, st);
                //scrive uno spazio come separatore
                generator.Emit(OpCodes.Ldstr, " ");
                generator.Emit(OpCodes.Call, typeof(System.Console).GetMethod("Write", new Type[]{typeof(string)}));
            }
            generator.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new Type[0]));
        }
    }

    class ReadStatement : FStatement
    {
        public IdentifierList ids;

        public ReadStatement(IdentifierList ids, TextSpan span)
        {
            this.Span = span;
            this.ids = ids;
        }

        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Read statement");
            ids.Print(tabs + 1);
        }
    }
}