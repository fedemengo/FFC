using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

using FFC.FAST;
using FFC.FGen;

namespace FFC.FGen
{
    public partial class Generator
    {
        public static SortedSet<string> GetUndeclared(StatementList list, SortedSet<string> declared)
        {
            var ans = new SortedSet<string>();
            foreach(FStatement f in list.StmsList)
            {
                if(f is DeclarationStatement)
                {
                    DeclarationStatement d = f as DeclarationStatement;
                    declared.Add(d.Id.Name);
                    AddUndeclared(d.Expr, declared, ans);
                }
                else if(f is AssignmentStatement)
                {
                    AssignmentStatement a = f as AssignmentStatement;
                    AddUndeclared(a.Left, declared, ans);
                    AddUndeclared(a.Right, declared, ans);
                }
                else if(f is ExpressionStatement)
                    AddUndeclared((f as ExpressionStatement).Expr, declared, ans);
                else if(f is FunctionCallStatement)
                    AddUndeclared((f as FunctionCallStatement).Func, declared, ans);
                else if(f is IfStatement)
                {
                    IfStatement i = f as IfStatement;
                    //add all conditions and merge all bodies
                    AddUndeclared(i.Condition, declared, ans);
                    var ifBody = GetUndeclared(i.Then, declared);
                    ans = Merge(ans, ifBody);
                    //else ifs
                    foreach(var ei in i.ElseIfs.ElseIfStmsList)
                    {
                        AddUndeclared(ei.Condition, declared, ans);
                        var body = GetUndeclared(ei.Then, declared);
                        ans = Merge(ans, body);
                    }
                    //else, no condition
                    var elseBody = GetUndeclared(i.Else, declared);
                    ans = Merge(ans, elseBody);
                }
                else if(f is LoopStatement)
                {
                    LoopStatement l = f as LoopStatement;
                    //add header, then body
                    if(l.Header is ForHeader)
                    {
                        AddUndeclared((l.Header as ForHeader).Collection, declared, ans);
                        //todo: we dont want recursive forheader (for x in v[x]), think about how to handle
                        declared.Add((l.Header as ForHeader).Id.Name);
                    }
                    if(l.Header is WhileHeader)
                        AddUndeclared((l.Header as WhileHeader).Condition, declared, ans);
                    //body
                    ans = Merge(ans, GetUndeclared(l.Body, declared));
                }
                else if(f is ReturnStatement)
                    AddUndeclared((f as ReturnStatement).Value, declared, ans);
                else if(f is BreakStatement || f is ContinueStatement)
                    continue;
                else if(f is PrintStatement)
                {
                    foreach(var e in (f as PrintStatement).ToPrint.Exprs)
                        AddUndeclared(e, declared, ans);
                }
                else throw new FCompilationException($"{f.Span} - Capturing not implemented for {f.GetType().Name}");
            }
            return ans;
        }
        public static void AddUndeclared(FExpression e, SortedSet<string> declared, SortedSet<string> ans)
        {
            if(e == null) return;
            else if(e is BinaryOperatorExpression)
            {
                AddUndeclared((e as BinaryOperatorExpression).Left, declared, ans);
                AddUndeclared((e as BinaryOperatorExpression).Right, declared, ans);
            }
            else if(e is EllipsisExpression)
            {
                AddUndeclared((e as EllipsisExpression).From, declared, ans);
                AddUndeclared((e as EllipsisExpression).To, declared, ans);                
            }
            else if(e is NegativeExpression)
                AddUndeclared((e as NegativeExpression).Value, declared, ans);
            else if(e is FunctionCall)
            {
                //caller and parameters
                AddUndeclared((e as FunctionCall).ToCall, declared, ans);
                foreach(var x in (e as FunctionCall).ExprsList.Exprs)
                    AddUndeclared(x, declared, ans);
            }
            else if(e is IndexedAccess)
            {
                var i = e as IndexedAccess;
                AddUndeclared(i.Container, declared, ans);
                //we ignore dot indexers
                if(i.Index is SquaresIndexer)
                    AddUndeclared((i.Index as SquaresIndexer).IndexExpr, declared, ans);
            }
            else if(e is FValue)
            {
                //if this Identifier was never used and isn't a standard function
                if(e is Identifier)
                    if(declared.Contains((e as Identifier).Name) == false && StandardFunctions.Funcs.ContainsKey((e as Identifier).Name) == false)
                        ans.Add((e as Identifier).Name);
            }
            else if(e is Conditional)
            {
                var c = e as Conditional;
                AddUndeclared(c.Condition, declared, ans);
                AddUndeclared(c.IfFalse, declared, ans);
                AddUndeclared(c.IfTrue, declared, ans);
            }
            else if(e is FunctionDefinition)
            {
                var f = e as FunctionDefinition;
                //Add params
                foreach(var x in f.ParamsList.Params)
                    declared.Add(x.Id.Name);
                //merge with body
                ans = Merge(ans, GetUndeclared(f.Body, declared));
            }
            else if(e is ArrayDefinition)
            {
                foreach(var x in (e as ArrayDefinition).ExprsList.Exprs)
                    AddUndeclared(x, declared, ans);
            }
            else if(e is MapDefinition)
            {
                foreach(var x in (e as MapDefinition).Entries.pairs)
                {
                    AddUndeclared(x.First, declared, ans);
                    AddUndeclared(x.Second, declared, ans);       
                }
            }
            else if(e is TupleDefinition)
            {
                foreach(var x in (e as TupleDefinition).ElemsList.Elements)
                    AddUndeclared(x.Value, declared, ans);                    
            }
            else if(e is ReadExpression)
                return;
            else throw new FCompilationException($"{e.Span} - Capturing not implemented for {e.GetType().Name}");
        }
        public static SortedSet<string> Merge(SortedSet<string> a, SortedSet<string> b)
        {
            if(a.Count < b.Count) return Merge(b, a);
            foreach(string x in b)
                a.Add(x);
            return a;
        }
    }
}