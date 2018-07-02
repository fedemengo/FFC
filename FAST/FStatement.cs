using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
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
            AssignmentStatement
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
        public List<FStatement> StmsList {get; set;}
        public StatementList(FStatement statement, TextSpan span)
        {
            StmsList = new List<FStatement>{statement};
            Span = span;
        }
        public StatementList(TextSpan span)
        {
            StmsList = new List<FStatement>();
            Span = span;
        }
        public void Add(FStatement stm) => StmsList.Add(stm);
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Statement list");
            foreach(FStatement fs in StmsList)
                fs.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            foreach(FStatement stm in StmsList)
            {
                if (stm is DeclarationStatement) 
                    stm.Generate(generator, currentType, ref st, exitLabel, conditionLabel);
                else 
                    stm.Generate(generator, currentType, st, exitLabel, conditionLabel);
            }
        }

        public override void BuildValueType(SymbolTable st)
        {
            //flag to avoid operations after return
            bool returned = false;
            foreach(var stm in StmsList)
            {
                if(returned) throw new FCompilationException($"{stm.Span} - Can't have operations after a return");
                if(stm is ReturnStatement) returned = true;
                if(stm is DeclarationStatement)
                {
                    var x = stm as DeclarationStatement;
                    st = st.Assign(x.Id.Name, new NameInfo(null, x.GetValueType(st)));
                    continue;
                }
                //statements that we can safely skip
                if(stm is AssignmentStatement || stm is FunctionCallStatement || stm is ContinueStatement || stm is BreakStatement || stm is PrintStatement)
                    continue;
                else
                {
                    var x = stm.GetValueType(st);
                    if(ValueType == null) ValueType = x;
                    else if(x != null && FType.SameType(x, ValueType) == false)
                        throw new FCompilationException($"{Span} - Can't deduce type as {ValueType} is not compatible with {x} at {stm.Span}");
                }
            }
        }
    }
    public class ExpressionStatement : FStatement
    {
        public FExpression Expr {get; set;}
        public ExpressionStatement(FExpression expression, TextSpan span)
        {
            Expr = expression;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Expression statement");
            Expr.Print(tabs + 1);
        }
        public override void BuildValueType(SymbolTable st) => ValueType = Expr.GetValueType(st);
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            Expr.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Ret);
        }        
    }
    public class FunctionCallStatement : FStatement
    {
        public FunctionCall Func {get; set;}
        public FunctionCallStatement(FunctionCall function, TextSpan span)
        {
            Func = function;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("FunctionCall statement");
            Func.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            Func.Generate(generator, currentType, st, exitLabel, conditionLabel);
            //pop result if not void
            if(Func.GetValueType(st) is VoidType == false) generator.Emit(OpCodes.Pop);
        }
    }
    public class AssignmentStatement : FStatement
    {
        public FSecondary Left {get; set;}
        public FExpression Right {get; set;}
        public AssignmentStatement(FSecondary left, FExpression right, TextSpan span)
        {
            Left = left;
            Right = right;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Assignment statement");
            Left.Print(tabs + 1);
            Right.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            //Empty array assignment
            if(Right is ArrayDefinition && Left.GetValueType(st) is ArrayType)
                (Right as ArrayDefinition).SetEmpty((Left.GetValueType(st)));

            if(Left is Identifier)
            {
                Identifier leftID = Left as Identifier;
                var definedSymbol = st.Find(leftID.Name);
                if(definedSymbol == null) 
                    throw new FCompilationException($"{Span} - Identifier {(Left as Identifier).Name} is not declared");
                                
                if(FType.SameType(Right.GetValueType(st), definedSymbol.Type) == false) 
                    throw new FCompilationException($"{Span} - Can't assign type {Right.GetValueType(st)} to variable of type {definedSymbol.Type}"); 
                
                //Empty array on identifier
                Right.Generate(generator, currentType, st, exitLabel, conditionLabel);
                Generator.EmitStore(generator, definedSymbol.Builder);
            }
            else if(Left is IndexedAccess)
            {
                FSecondary collection = (Left as IndexedAccess).Container;
                Indexer indexer = (Left as IndexedAccess).Index;
                //Empty array on index access to something
                if(collection.GetValueType(st) is ArrayType && FType.SameType((collection.GetValueType(st) as ArrayType).Type, Right.GetValueType(st)) == false)
                    throw new FCompilationException($"{Span} - Can't assign {Right.GetValueType(st)} to {collection}[{(collection.GetValueType(st) as ArrayType).Type}]");

                if(indexer is SquaresIndexer)
                { 
                    collection.Generate(generator, currentType, st, exitLabel, conditionLabel);
                    indexer.Generate(generator, currentType, st, exitLabel, conditionLabel);
                    Right.Generate(generator, currentType, st, exitLabel, conditionLabel);
                    generator.Emit(OpCodes.Callvirt, collection.GetValueType(st).GetRunTimeType().GetMethod("set_Item", new Type[]{indexer.GetValueType(st).GetRunTimeType(), Right.GetValueType(st).GetRunTimeType()}));
                }
                else if(indexer is DotIndexer)
                {
                    IntegerValue tupleIndex = new IntegerValue((collection.GetValueType(st) as TupleType).GetMappedIndex(indexer as DotIndexer), Span);
                    if(collection.GetValueType(st) is TupleType && FType.SameType((collection.GetValueType(st) as TupleType).TypesList.Types[tupleIndex.Value - 1], Right.GetValueType(st)) == false)
                        throw new FCompilationException($"{Span} - Can't assign {Right.GetValueType(st)} to {(collection.GetValueType(st) as TupleType).TypesList.Types[tupleIndex.Value - 1]}");

                    collection.Generate(generator, currentType, st, exitLabel, conditionLabel);
                    tupleIndex.Generate(generator, currentType, st, exitLabel, conditionLabel);
                    Right.Generate(generator, currentType, st, exitLabel, conditionLabel);
                    generator.Emit(OpCodes.Callvirt, collection.GetValueType(st).GetRunTimeType().GetMethod("Set", new Type[]{typeof(FInteger), Right.GetValueType(st).GetRunTimeType()}));
                }
                else
                    throw new FCompilationException($"{Span} - Assignments generation not supported for {indexer.GetType().Name}");
            }
            else throw new FCompilationException($"{Span} - Assignments to {Left.GetType().Name} are not implemented");
        }
    }
    public class DeclarationStatement : FStatement
    {
        public Identifier Id {get; set;}
        public FType Type {get; set;}
        public FExpression Expr {get; set;}
        public DeclarationStatement(Identifier id, FType type, FExpression expr, TextSpan span)
        {
            Id = id;
            Type = type;
            Expr = expr;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Declaration statement");
            Id.Print(tabs + 1);
            if(Type != null) Type.Print(tabs + 1);
            Expr.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, ref SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            /*  This is meant to cover recursive functions
                Shall we use a new FType or is null ok?
                Main idea is to set id to a null Value.
                as return expressions, null values are ignored.
                we just need to ignore them also in operators [TODO] and func call
                think if it's possible to support this on array[func], func tuples and func maps
            */
            st = st.Assign(Id.Name, new NameInfo(null, null));
            
            FType t = Expr.GetValueType(st);

            if(Type != null && FType.SameType(Type, t) == false)
                throw new FCompilationException($"{Span} - Type {t.GetRunTimeType().Name} doesn't match declaration {Type.GetRunTimeType().Name}");
            
            //Field when emitting locals in program type
            object builder;
            if(currentType == Generator.programType) builder = currentType.DefineField(Id.Name, t.GetRunTimeType(), FieldAttributes.Public | FieldAttributes.Static);
            else builder = generator.DeclareLocal(t.GetRunTimeType());
            
            st = st.Assign(Id.Name, new NameInfo(builder, t));
            
            Expr.Generate(generator, currentType, st, exitLabel, conditionLabel);
            Generator.EmitStore(generator, builder);
        }
        //Shall declaration have types?
        public override void BuildValueType(SymbolTable st)
        {
            //build type for empty arrays (0 elements)
            if(Expr is ArrayDefinition && (Expr as ArrayDefinition).ExprsList.Exprs.Count == 0)
            {
                if(Type == null)
                    throw new FCompilationException($"{Span} - Can't create empty array without specifying type");
                (Expr as ArrayDefinition).SetEmpty(Type);
            }
            
            //sets value type
            ValueType = Type;
            
            //If types are matching
            var t = Expr.GetValueType(st);
            bool sameType = false;
            if(Type == null || FType.SameType(t, Type))
            {
                sameType = true;    
                ValueType = Expr.GetValueType(st);
            }
            else if(t is FunctionType && Type is FunctionType)
            {
                //Special case for functions
                Type d1 = t.GetRunTimeType().BaseType;
                Type d2 = Type.GetRunTimeType().BaseType;
                sameType = d1 == d2;
            }
            if(!sameType)
                throw new FCompilationException($"{Span} - Type mismatch in variable {Id.Name}, {Expr.GetValueType(st).GetRunTimeType().Name} is not {Type.GetRunTimeType().Name}");
        }
    }
    public class DeclarationStatementList : FASTNode
    {
        public List<DeclarationStatement> DeclStmsList {get; set;}
        public DeclarationStatementList(DeclarationStatement stm, TextSpan span)
        {
            DeclStmsList = new List<DeclarationStatement>{stm};
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Declaration statement list");
            foreach(DeclarationStatement stm in DeclStmsList)
                stm.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            bool startEmitted = false;
            foreach(var x in DeclStmsList)
            {
                x.Generate(generator, currentType, ref st);
                if(x.Id.Name == Generator.StartFunction)
                {
                    if(startEmitted) throw new FCompilationException($"{Span} - Cannot define multiple functions as starting ones");
                    if(x.Expr is FunctionDefinition == false || (x.Expr as FunctionDefinition).GetValueType(st) is FunctionType == false)
                        throw new FCompilationException($"{x.Span} - Declaration not valid as start function");
                    Generator.EmitStartFunction(st.Find(x.Id.Name).Builder, (x.Expr as FunctionDefinition).GetValueType(st) as FunctionType);
                    startEmitted = true;
                }
            }
            if(!startEmitted) throw new FCompilationException($"Cannot compile a program without a starting function");
        }
        public void Add(DeclarationStatement stm) => DeclStmsList.Add(stm);
    }
    public class IfStatement : FStatement
    {
        public FExpression Condition {get; set;}
        public StatementList Then {get; set;}
        public ElseIfList ElseIfs {get; set;}
        public StatementList Else {get; set;}
        public IfStatement(FExpression condition, StatementList thenStm, ElseIfList elseIfs, StatementList elseStm, TextSpan span)
        {
            Condition = condition;
            Then = thenStm;
            ElseIfs = elseIfs;
            Else = elseStm;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("If statement");
            Condition.Print(tabs + 1);
            Then.Print(tabs + 1);
            ElseIfs.Print(tabs + 1);
            Else.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            if(Condition.GetValueType(st) is BooleanType == false)
                throw new FCompilationException($"{Span} - Can't use {Condition.GetValueType(st)} as condition");

            //Branch to the end of IfStatement
            Label exitBranch = generator.DefineLabel();
            
            //Emit if condition
            Condition.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("GetBool"));
            //Skip to next condition
            Label falseBranch = generator.DefineLabel();
            generator.Emit(OpCodes.Brfalse, falseBranch);

            //Generate then code
            Then.Generate(generator, currentType, st, exitLabel, conditionLabel);
            //After then, skip to end
            generator.Emit(OpCodes.Br, exitBranch);
            //Mark next check
            generator.MarkLabel(falseBranch);

            //Generate all of the else ifs
            foreach(var ei in ElseIfs.ElseIfStmsList)
            {
                //Check if condition is boolean
                if(ei.Condition.GetValueType(st) is BooleanType == false)
                    throw new FCompilationException($"{Span} - Can't use {Condition.GetValueType(st)} as condition");
                //Emit ElseIF condition
                ei.Condition.Generate(generator, currentType, st, exitLabel, conditionLabel);
                generator.Emit(OpCodes.Callvirt, typeof(FBoolean).GetMethod("GetBool"));
                //Skip to next condition
                Label nextElse = generator.DefineLabel();
                generator.Emit(OpCodes.Brfalse, nextElse);
                //Generate ElseIf body
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
            //to be sure label will get emitted (this is weird!) todo : check for more info why the simple return ain't enough
            generator.Emit(OpCodes.Nop);
        }

        public override void BuildValueType(SymbolTable st)
        {
            FType t = Then.GetValueType(st);
            FType ei = ElseIfs.GetValueType(st);
            FType e = Else.GetValueType(st);
            ValueType = t;
            if(ei != null)
                if(ValueType == null || FType.SameType(ValueType, ei))
                    ValueType = ei;
                else
                    throw new FCompilationException($"{Span} - If type {t.GetType().Name} doesn't match ElseIf type {ei.GetType().Name}");
            
            if(e != null)
                if(ValueType == null || FType.SameType(ValueType, e))
                    ValueType = e;
                else
                    throw new FCompilationException($"{Span} - If type {t.GetType().Name} doesn't match Else type {e.GetType().Name}");
        }
    }

    public class ElseIfList : FASTNode
    {
        public List<ElseIfStatement> ElseIfStmsList {get; set;}
        public void Add(ElseIfStatement other) => ElseIfStmsList.Add(other);
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Else if list");
            foreach(var l in ElseIfStmsList)
                l.Print(tabs + 1);
        }
        public ElseIfList(TextSpan span = null)
        {
            ElseIfStmsList = new List<ElseIfStatement>();
            Span = span;
        }
        public ElseIfList(ElseIfStatement start, TextSpan span)
        {
            ElseIfStmsList = new List<ElseIfStatement>{start};
            Span = span;
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            foreach(ElseIfStatement elif in ElseIfStmsList)
                elif.Generate(generator, currentType, st, exitLabel, conditionLabel);
        }
        public override void BuildValueType(SymbolTable st)
        {
            foreach(var x in ElseIfStmsList)
                if(ValueType == null || FType.SameType(ValueType, x.GetValueType(st)))
                    ValueType = x.GetValueType(st);
                else throw new FCompilationException($"{Span} - Return type mismatch {ValueType} vs {x.GetValueType(st)}");
        }
    }

    public class ElseIfStatement : FASTNode
    {
        public FExpression Condition {get; set;}
        public StatementList Then {get; set;}
        public ElseIfStatement(FExpression condition, StatementList then, TextSpan span)
        {
            Condition = condition;
            Then = then;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Else if");
            Condition.Print(tabs + 1);
            Then.Print(tabs + 1);
        }
        public override void BuildValueType(SymbolTable st) => ValueType = Then.GetValueType(st);
    }

    public class ReturnStatement : FStatement
    {
        public FExpression Value {get; set;}
        public ReturnStatement(FExpression value, TextSpan span)
        {
            Value = value;
            Span = span;
        }
        public ReturnStatement(TextSpan span) => this.Span = span;
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Return statement");
            if(Value != null)
                Value.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            if(Value != null) Value.Generate(generator, currentType, st, exitLabel, conditionLabel);
            generator.Emit(OpCodes.Ret);
        }
        public override void BuildValueType(SymbolTable st) => ValueType = (Value != null ? Value.GetValueType(st) : new VoidType());
    }
    public class BreakStatement : FStatement
    {
        public BreakStatement(TextSpan span) => this.Span = span;
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Break statement");
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel, Label conditionLabel)
        {
            generator.Emit(OpCodes.Br, exitLabel);
        }
    }
    public class ContinueStatement : FStatement
    {
        public ContinueStatement(TextSpan span) => this.Span = span;
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Continue statement");
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel, Label conditionLabel)
        {
            generator.Emit(OpCodes.Br, conditionLabel);
        }
    }
    public class PrintStatement : FStatement
    {
        public ExpressionList ToPrint {get; set;}
        public PrintStatement(ExpressionList toPrint, TextSpan span)
        {
            ToPrint = toPrint;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Print statement");
            ToPrint.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            for(int i = 0; i < ToPrint.Exprs.Count - 1; i++)
            {
                FExpression expr = ToPrint.Exprs[i];
                expr.EmitPrint(generator, currentType, st);
                //scrive uno spazio come separatore
                generator.Emit(OpCodes.Ldstr, " ");
                generator.Emit(OpCodes.Call, typeof(Console).GetMethod("Write", new Type[]{typeof(string)}));
            }
            ToPrint.Exprs[ToPrint.Exprs.Count - 1].EmitPrint(generator, currentType, st, true);
        }
    }
}