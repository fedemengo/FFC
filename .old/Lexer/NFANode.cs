using System;
using System.Collections.Generic;

namespace Lexer
{
    class NFANode
    {
        public List<NFANode> freeLinks {get; set;} = new List<NFANode>();
        public Dictionary<char, NFANode> links {get; set;} = new Dictionary<char, NFANode>();

        public void Insert(KeyValuePair<char, NFANode> edge)
        {
            if(edge.Key == '\0')
                freeLinks.Add(edge.Value);
            else
                links.Add(edge.Key, edge.Value);
        }
    }
}
