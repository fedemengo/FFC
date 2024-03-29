using FFC;
using FFC.FLexer;
using FFC.FGen;

namespace FFC.FParser
{
    public abstract class TValue
    {
        public TextSpan Span {get; set;}
    }
    
    public class TextSpan
    {
        public Position Begin {get; set;}
        
        public Position End {get; set;}
        
        public TextSpan(Position b, Position e)
        {
            Begin = b;
            End = e;
        }
        
        public TextSpan MergeTo(TextSpan other) => new TextSpan(this, other);
        
        public TextSpan(TextSpan t1) : this(t1.Begin, t1.End) {}
        
        static TextSpan Merge(TextSpan t1, TextSpan t2) => new TextSpan(t1, t2);
        
        public TextSpan(TextSpan t1, TextSpan t2) : this(t1 != null ? t1.Begin : t2.Begin, t2 != null ? t2.End : t1.End) {}

        
        public override string ToString() => Begin.ToString() + ", " + End.ToString();
    }
}