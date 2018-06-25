using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using FFC.FParser;
using FFC.FRunTime;
using FFC.FGen;

namespace FFC.FAST
{
    public abstract class FStatement : FASTNode
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
    public class StatementList : FASTNode
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
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            foreach(FStatement stm in statements)
            {
                if (stm is DeclarationStatement) (stm as DeclarationStatement).Generate(generator, currentType, ref st, exitLabel, conditionLabel);
                else stm.Generate(generator, currentType, st, exitLabel, conditionLabel);
            }
        }

        public override void BuildType(SymbolTable st)
        {
            foreach(var stm in statements)
            {
                if(stm is DeclarationStatement)
                {
                    var x = stm as DeclarationStatement;
                    st = st.Assign(x.id.name, new NameInfo(null, x.GetValueType(st)));
                    continue;
                }
                //statements that we can safely skip
                if(stm is AssignmentStatemt || stm is FunctionCallStatement || stm is ContinueStatement || stm is BreakStatement || stm is PrintStatement)
                    continue;
                else
                {
                    var x = stm.GetValueType(st);
                    if(valueType == null) valueType = x;
                    else throw new NotImplementedException($"{Span} - Can't deduce type as {valueType} is not compatible with {x} at {stm.Span}");
                }
            }
        }
    }
    public class ExpressionStatement : FStatement
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
    public class FunctionCallStatement : FStatement
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
    public class AssignmentStatemt : FStatement
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
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            //Empty array assignment
            if(right is ArrayDefinition && left.GetValueType(st) is ArrayType)
            {
                (right as ArrayDefinition).SetEmpty((left.GetValueType(st)));
            }
            if(left is Identifier)      // get
            {
                Identifier x = left as Identifier;
                var y = st.Find(x.name);
                if(y == null) throw new NotImplementedException($"{Span} - Identifier {(left as Identifier).name} is not declared");
                if(right.GetValueType(st).GetRunTimeType() != y.Type.GetRunTimeType()) throw new NotImplementedException($"{Span} - Can't assign type {right.GetValueType(st).GetRunTimeType()} to variable of type {x.GetValueType(st).GetRunTimeType()}"); 
                //Empty array on identifier
                right.Generate(generator, currentType, st, exitLabel, conditionLabel);
                generator.Emit(OpCodes.Stloc, y.LocBuilder);
            }
            else if(left is IndexedAccess)      // set
            {
                IndexedAccess x = left as IndexedAccess;
                FType collection = x.container.GetValueType(st);
                //Empty array on index access to something
                if(collection is ArrayType && (collection as ArrayType).type.GetRunTimeType() != right.GetValueType(st).GetRunTimeType())
                {
                    FType element = right.GetValueType(st);
                    throw new NotImplementedException($"{Span} - Can't assign {element.GetRunTimeType().Name} to {collection.GetRunTimeType().Name}[{(collection as ArrayType).type.GetRunTimeType().Name}]");
                }
                x.container.Generate(generator, currentType, st, exitLabel, conditionLabel);
                x.index.Generate(generator, currentType, st, exitLabel, conditionLabel);
                //generate expression to load
                right.Generate(generator, currentType, st, exitLabel, conditionLabel);
                //ckeck types - currently too sleepy!
                if(x.index is SquaresIndexer)
                    generator.Emit(OpCodes.Callvirt, x.container.GetValueType(st).GetRunTimeType().GetMethod("set_Item", new Type[]{x.index.GetValueType(st).GetRunTimeType(), right.GetValueType(st).GetRunTimeType()}));
                else
                    throw new NotImplementedException($"{Span} - Generation not supported for {x.index.GetType().Name}");
            }
            else throw new NotImplementedException($"{Span} - Assignments to {left.GetType().Name} are not implemented");
        }
    }
    public class DeclarationStatement : FStatement
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

        public override void Generate(ILGenerator generator, TypeBuilder currentType, ref SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            FType t = null;
            //Empty array
            if(expr is ArrayDefinition && (expr as ArrayDefinition).values.expressions.Count == 0)
            {
                if(type == null)
                    throw new NotImplementedException($"{Span} - Can't create empty array without specifying type");
                t = type;
                (expr as ArrayDefinition).SetEmpty(t);
            }
            else
                t = expr.GetValueType(st);
            if(type != null && type.GetRunTimeType() != t.GetRunTimeType())
                throw new NotImplementedException($"{Span} - Type {t.GetRunTimeType().Name} doesn't match declaration {type.GetRunTimeType().Name}");
            LocalBuilder var = generator.DeclareLocal(t.GetRunTimeType());
            st = st.Assign(id.name, new NameInfo(var, t));
            expr.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Stloc, var);
        }
        //Shall declaration have types?
        public override void BuildType(SymbolTable st)
        {
            valueType = type;
            if(type == null || expr.GetValueType(st) == type)
                valueType = expr.GetValueType(st);
            else
                throw new NotImplementedException($"{Span} - Type mismatch in variable {id.name}, {expr.GetValueType(st)} is not {type}");
        }
    }
    public class DeclarationStatementList : FASTNode
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
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            foreach(var x in statements)
                x.Generate(generator, currentType, ref st);
        }
        public void Add(DeclarationStatement stm)
        {
            statements.Add(stm);
        }
    }
    public class IfStatement : FStatement
    {
        public FExpression condition;
        public StatementList Then;
        public ElseIfList ElseIfs;
        public StatementList Else;
        public IfStatement(FExpression condition, StatementList Then, ElseIfList ElseIfs, StatementList Else, TextSpan span)
        {
            this.Span = span;
            this.condition = condition;
            this.Then = Then;
            this.ElseIfs = ElseIfs;
            this.Else = Else;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("If statement");
            condition.Print(tabs + 1);
            Then.Print(tabs + 1);
            ElseIfs.Print(tabs + 1);
            Else.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            if(condition.GetValueType(st).GetRunTimeType() != typeof(FBoolean))
                throw new NotImplementedException($"{Span} - Can't use {condition.GetValueType(st)} as condition");

            //Branch to the end of IfStatement
            Label exitBranch = generator.DefineLabel();
            
            //Emit if condition
            condition.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
            //Skip to next condition
            Label falseBranch = generator.DefineLabel();
            Then.Generate(generator, currentType, st, exitLabel, conditionLabel);
            //After then, skip to end
            generator.Emit(OpCodes.Br, exitBranch);
            //Mark next check
            generator.MarkLabel(falseBranch);

            //Generate all of the else ifs
            foreach(var ei in ElseIfs.list)
            {
                //Check if condition is boolean
                if(ei.condition.GetValueType(st) is BooleanType == false)
                    throw new NotImplementedException($"{Span} - Can't use {condition.GetValueType(st)} as condition");
                //Emit ElseIF condition
                ei.condition.Generate(generator, currentType, st, exitLabel, conditionLabel);
                generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("get_Value"));
                //Skip to next condition
                Label nextElse = generator.DefineLabel();
                ei.Then.Generate(generator, currentType, st, exitLabel, conditionLabel);
                //Skip to end
                generator.Emit(OpCodes.Br, exitBranch);
                //Mark next check
                generator.MarkLabel(nextElse);
            }

            //Generate code for else
            Else.Generate(generator, currentType, st, exitLabel, conditionLabel);

            //End of IfStatement
            generator.MarkLabel(exitBranch);

        }

        public override void BuildType(SymbolTable st)
        {
            FType t = Then.GetValueType(st);
            FType ei = ElseIfs.GetValueType(st);
            FType e = Else.GetValueType(st);
            valueType = t;
            if(ei != null)
                if(valueType == null || valueType.GetRunTimeType() == ei.GetRunTimeType())
                    valueType = ei;
                else
                    throw new NotImplementedException($"{Span} - If type {t.GetType().Name} doesn't match ElseIf type {ei.GetType().Name}");
            
            if(e != null)
                if(valueType == null || valueType.GetRunTimeType() == e.GetRunTimeType())
                    valueType = e;
                else
                    throw new NotImplementedException($"{Span} - If type {t.GetType().Name} doesn't match Else type {e.GetType().Name}");
        }

    }

    public class ElseIfList : FASTNode
    {
        public List<ElseIfStatement> list;
        public void Add(ElseIfStatement other) => list.Add(other);
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

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            foreach(ElseIfStatement elif in list)
                elif.Generate(generator, currentType, st, exitLabel, conditionLabel);
        }
        public override void BuildType(SymbolTable st)
        {
            foreach(var x in list)
                if(valueType == null || valueType == x.GetValueType(st))
                    valueType = x.GetValueType(st);
                else throw new NotImplementedException($"{Span} - Return type mismatch");
        }
    }

    public class ElseIfStatement : FASTNode
    {
        public FExpression condition;
        public StatementList Then;
        public ElseIfStatement(FExpression condition, StatementList Then, TextSpan span)
        {
            this.Span = span;
            this.condition = condition;
            this.Then = Then;
        }

        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Else if");
            condition.Print(tabs + 1);
            Then.Print(tabs + 1);
        }
        public override void BuildType(SymbolTable st) => valueType = Then.GetValueType(st);
    }

    public class ReturnStatement : FStatement
    {
        public FExpression value = null;
        public ReturnStatement(FExpression value, TextSpan span)
        {
            this.Span = span;
            this.value = value;
        }
        public ReturnStatement(TextSpan span) => this.Span = span;
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Return statement");
            if(value != null)
                value.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            if(value != null) value.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Ret);
        }
        public override void BuildType(SymbolTable st) => valueType = value.GetValueType(st);
    }
    public class BreakStatement : FStatement
    {
        public BreakStatement(TextSpan span) => this.Span = span;
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Break statement");
        }

        public void Generate(ILGenerator generator, Label exitLabel) => generator.Emit(OpCodes.Br, exitLabel);
    }
    public class ContinueStatement : FStatement
    {
        public ContinueStatement(TextSpan span) => this.Span = span;
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Continue statement");
        }
        public void Generate(ILGenerator generator, Label conditionLabel) => generator.Emit(OpCodes.Br, conditionLabel);
    }
    public class PrintStatement : FStatement
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

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            foreach(FExpression expr in toPrint.expressions){
                expr.EmitPrint(generator, currentType, st);
                //scrive uno spazio come separatore
                generator.Emit(OpCodes.Ldstr, " ");
                generator.Emit(OpCodes.Call, typeof(System.Console).GetMethod("Write", new Type[]{typeof(string)}));
            }
            generator.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new Type[0]));
        }
    }
}