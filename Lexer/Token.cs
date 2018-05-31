using System.Collections.Generic;
namespace Lexer
{
    class Token
    {
        public ETokens type{get; set;}
        public List<object> values = new List<object>();
        //Position is in a [L, R) format
        public Position begin;
        public Position end;
        public Token(ETokens tokenType, Position begin, Position end)
        {
            this.type = tokenType;
            this.begin = begin;
            this.end = end;
        }

        public Token(ETokens tokenType, List<object> objs, Position begin, Position end) : this(tokenType, begin, end)
        {
            foreach(object o in objs)
                values.Add(o);
        }

        override public string ToString()
        {
            string ans = type.ToString();
            for(int i = 0; i < values.Count; i++)
            {
                if(i == 0)
                    ans += "(";
                else
                    ans += ", ";
                ans += values[i].ToString();
                if(i == values.Count - 1)
                    ans += ")";
            }
            ans += " " + begin.ToString() + ", " + end.ToString();
            return ans;
        }
    }
}