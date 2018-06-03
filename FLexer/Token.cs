using System.Collections.Generic;
using System;
using FFC.FParser;
namespace FFC.FLexer
{
    class TokenValue : TValue
    {
        //wraps around list
        private List<object> values = new List<object>();
        public void Add(object o)
        {
            values.Add(o);
        }
        public int Count
        {
            get => values.Count;
        }
        public object this[int i]
        {
            get => values[i];
            set => values[i] = value;
        }

    }
    class Token
    {
        public ETokens type{get; set;}
        public TokenValue values = new TokenValue();
        //Position is in a [L, R) format
        public QUT.Gppg.LexLocation span;
        public Token(ETokens tokenType, Position begin, Position end)
        {
            Console.WriteLine("Token : " + tokenType);
            this.type = tokenType;
            this.span = new QUT.Gppg.LexLocation(begin.Row, begin.Column, end.Row, end.Column);
        }

        public Token(ETokens tokenType, List<object> objs, Position begin, Position end) : this(tokenType, begin, end)
        {
            foreach(object o in objs)
                values.Add(o);
            Console.Write("\t");
            foreach(object o in objs)
                Console.Write(o.ToString());
            Console.WriteLine();
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
            ans += " " + span.ToString();
            return ans;
        }
    }
}