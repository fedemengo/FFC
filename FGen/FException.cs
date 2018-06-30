using System;

namespace FFC.FGen
{
    public abstract class FException : Exception
    {
<<<<<<< a311fc16eec1e72fb0b16456a98907f88bed45d4
        public new string Message {get; set;}
        public FException(string s) => Message = s;
=======
        public FException(string s) : base(s) {}
>>>>>>> Change properties to fields in runtime classes
    }
    public class FCompilationException : FException
    {
        public FCompilationException(string s) : base(s) {}
    }
}