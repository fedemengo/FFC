namespace naiveLex
{
    public class TokenDefinition
    {
        private TokenType tokenType;
        private Regex tokenRegex;
        private int tokenPrecedence;

        public TokenDefinition(TokenType type, Regex regex, int precedence)
        {
            this.tokenType = type;
            this.regex = tokenRegex;
            this.tokenPrecedence = precedence;
        }

        public IEnumerable<TokenMatch> AllMatches(string text)
        {
            foreach(Match match : this.regex.Matches(text))
            {
                yield return new TokenMatch(tokenType, tokenPrecedence, match.Index, match.Length);
            }
        }
    }

    public class TokenMatch
    {
        private TokenType tokenType;
        private int precedence;
        private int index;
        private int length;

        public TokenMatch(TokenType type, int precedence, int index , int length)
        {
            this.tokenType = type;
            this.precedence = precedence;
            this.index = index;
            this.length = length;
        }

        public TokenType Type { get; private set; }
        public int Precedence { get; private set; }
        public int Index { get; private set; }
        public int Length { get; private set; }
    }
}