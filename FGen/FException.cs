using System;

namespace FFC.FGen
{
    public abstract class FException : Exception
    {
        public new string Message {get; set;}
        public FException(string s) => Message = s;
    }
    public class FCompilationException : FException
    {
        public FCompilationException(string s) : base(s) {}
    }
}