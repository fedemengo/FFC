using System.Collections.Generic;

namespace Lexer
{
    class Token
    {
        public ETokens type{get; set;}
        public List<object> values = new List<object>();
        public Position position;
        public Token(ETokens tokenType, Position pos)
        {
            this.type = tokenType;
            this.position = pos;
        }

        public Token(ETokens tokenType, List<object> objs, Position pos) : this(tokenType, pos)
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
            ans += " [" + position.Row + "," + position.Column + "]";
            return ans;
        }
    }
}