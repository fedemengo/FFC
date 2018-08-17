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
			{"norm", new RationalType()}    //Normalizes the rational number
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

	}
}