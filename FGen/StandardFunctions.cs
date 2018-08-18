using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using FFC.FAST;
using FFC.FRunTime;

namespace FFC.FGen
{
	public class StandardFunctions
	{
		public static Dictionary<string, FType> Funcs{get; set;} = new Dictionary<string, FType>
		{
			{"length", new IntegerType()},  //Array and map length
			{"round", new IntegerType()},   //Rounds to integer a real or a rational
			{"re", new RealType()},         //Real part of a complex number
			{"im", new RealType()},         //Imaginary part of a complex number
			{"num", new IntegerType()},     //Numerator of a rational
			{"denom", new IntegerType()},   //Denominator of a rational
			{"compl", new ComplexType()},   //Creates complex from two or one integer/real
			{"rat", new RationalType()},    //Creates a rational from two or one integer
			//{"norm", new RationalType()}    //As rational are normalized at each step, this function would be useless
		};

		//List of methods to Emit all custom functions
		public static void EmitLength(ExpressionList expr, ILGenerator generator, TypeBuilder currentType, SymbolTable st)
		{
			if(expr.Exprs.Count != 1)
				throw new FCompilationException($"{expr.Span} - Standard function length takes a single parameter");
			//The object we need to get length of
			var e = expr.Exprs[0];
			var t = e.GetValueType(st);
			if(t is ArrayType == false && t is MapType == false)
				throw new FCompilationException($"{expr.Span} - Standard function length cannot be used on type {t}");
			//We emit the object
			e.Generate(generator, currentType, st);
			//we call Length function
			generator.Emit(OpCodes.Callvirt, (t.GetRunTimeType()).GetMethod("GetLength"));
			//Length is now on the stack, ready to be used
		}

		public static void EmitRound(ExpressionList expr, ILGenerator generator, TypeBuilder currentType, SymbolTable st)
		{
			if(expr.Exprs.Count != 1)
				throw new FCompilationException($"{expr.Span} - Standard function round takes a single parameter");

			var e = expr.Exprs[0];
			var t = e.GetValueType(st);
			if(t is RealType == false && t is RationalType == false)			
				throw new FCompilationException($"{expr.Span} - Standard function round cannot be used on type {t}");
			// emit the object
			e.Generate(generator, currentType, st);
			// call Round on the type
			generator.Emit(OpCodes.Callvirt, (t.GetRunTimeType()).GetMethod("Round"));
			// the correct integer type is now on the stack
		}

		public static void EmitRat(ExpressionList expr, ILGenerator generator, TypeBuilder currentType, SymbolTable st)
		{
			if(expr.Exprs.Count != 1 && expr.Exprs.Count != 2)
				throw new FCompilationException($"{expr.Span} - Standard function rat takes one or two parameter");
			
			var num = expr.Exprs[0];
			var den = expr.Exprs.Count == 2 ? expr.Exprs[1] : new IntegerValue(1);	
			FType numType = num.GetValueType(st);
			FType denType = den.GetValueType(st);

			if(numType is IntegerType == false || denType is IntegerType == false)
				throw new FCompilationException($"{expr.Span} -  Standard function ran cannot be used on type {numType} and {denType}");

			num.Generate(generator, currentType, st);
			den.Generate(generator, currentType, st);
            generator.Emit(OpCodes.Call, typeof(FRational).GetMethod("Rat"));
		}

		public static void EmitCompl(ExpressionList expr, ILGenerator generator, TypeBuilder currentType, SymbolTable st)
		{
			if(expr.Exprs.Count != 1 && expr.Exprs.Count != 2)
				throw new FCompilationException($"{expr.Span} - Standard function compl takes one or two parameter");
			
			var real = expr.Exprs[0];
			var img = expr.Exprs.Count == 2 ? expr.Exprs[1] : new RealValue(0);
		
			FType realType = real.GetValueType(st);
			FType imgType = img.GetValueType(st);

			// the type is expected to be corrected at run time (Integer or Real)
			if(!((realType is RealType || realType is IntegerType) && (imgType is RealType || imgType is IntegerType)))
				throw new FCompilationException($"{expr.Span} -  Standard function compl cannot be used on type {realType} and {imgType}");

			real.Generate(generator, currentType, st);
			img.Generate(generator, currentType, st);
			generator.Emit(OpCodes.Call, typeof(FComplex).GetMethod("Compl"));
		}

		public static void EmitRe(ExpressionList expr, ILGenerator generator, TypeBuilder currentType, SymbolTable st)
		{
			if(expr.Exprs.Count > 1)
				throw new FCompilationException($"{expr.Span} - Standard function re takes a single parameter");
			var e = expr.Exprs[0];
			var t = e.GetValueType(st);
			if(t is ComplexType == false)			
				throw new FCompilationException($"{expr.Span} - Standard function re cannot be used on type {t}");
			e.Generate(generator, currentType, st);
			generator.Emit(OpCodes.Callvirt, (t.GetRunTimeType()).GetMethod("Re"));
		}

		public static void EmitIm(ExpressionList expr, ILGenerator generator, TypeBuilder currentType, SymbolTable st)
		{
			if(expr.Exprs.Count > 1)
				throw new FCompilationException($"{expr.Span} - Standard function im takes a single parameter");
			var e = expr.Exprs[0];
			var t = e.GetValueType(st);
			if(t is ComplexType == false)			
				throw new FCompilationException($"{expr.Span} - Standard function im cannot be used on type {t}");
			e.Generate(generator, currentType, st);
			generator.Emit(OpCodes.Callvirt, (t.GetRunTimeType()).GetMethod("Im"));
		}
		public static void EmitNum(ExpressionList expr, ILGenerator generator, TypeBuilder currentType, SymbolTable st)
		{
			if(expr.Exprs.Count > 1)
				throw new FCompilationException($"{expr.Span} - Standard function num takes a single parameter");
			var e = expr.Exprs[0];
			var t = e.GetValueType(st);
			if(t is RationalType == false)			
				throw new FCompilationException($"{expr.Span} - Standard function num cannot be used on type {t}");
			e.Generate(generator, currentType, st);
			generator.Emit(OpCodes.Callvirt, (t.GetRunTimeType()).GetMethod("Num"));
		}

		public static void EmitDenom(ExpressionList expr, ILGenerator generator, TypeBuilder currentType, SymbolTable st)
		{
			if(expr.Exprs.Count > 1)
				throw new FCompilationException($"{expr.Span} - Standard function denom takes a single parameter");
			var e = expr.Exprs[0];
			var t = e.GetValueType(st);
			if(t is RationalType == false)			
				throw new FCompilationException($"{expr.Span} - Standard function denom cannot be used on type {t}");
			e.Generate(generator, currentType, st);
			generator.Emit(OpCodes.Callvirt, (t.GetRunTimeType()).GetMethod("Denom"));
		}
	}
}