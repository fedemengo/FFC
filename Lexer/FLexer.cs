using System.Collections.Generic;

namespace Lexer
{
    class FLexer
    {
        static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
        static bool IsBlank(char c)
        {
            switch(c)
            {
                case ' ':
                case '\n':
                case '\t': return true;
                default: return false;
            }
        }
        static bool IsIdentifierChar(char c)
        {
            return IsLetter(c) || IsDigit(c) || c == '_';
        }
        static bool IsLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
        static double GetDouble(string before, string after)
        {
            return double.Parse(before + "." + after);
        }
        static int GetInt(string num)
        {
            return int.Parse(num);
        }
        static string GetDigits(SourceReader sr)
        {
            string tmp = "";
            while(IsDigit(sr.GetChar()))
            {
                tmp += sr.GetChar();
                sr.Advance();
            }
            return tmp;
        }
        public List<Token> GetTokens(SourceReader sr)
        {
            List<Token> ans = new List<Token>();
            Token current = null;
            while((current = NextToken(sr)).type != ETokens.EOF)
            {
                ans.Add(current);
                if(current.type == ETokens.ERROR)
                    sr.SkipBlank();
            }
                
            return ans;
        }
        Token NextToken(SourceReader sr)
        {
            if(sr.Empty()) return new Token(ETokens.EOF, sr.GetPosition());
            switch(sr.GetChar())
            {
                case ',' : sr.Advance(); return new Token(ETokens.COMMA, sr.GetPosition());
                case ';' : sr.Advance(); return new Token(ETokens.SEMICOLON, sr.GetPosition());
                case ':' :
                    sr.Advance();
                    if(sr.GetChar() == '=')
                    {
                        sr.Advance();
                        return new Token(ETokens.ASSIGN, sr.GetPosition());
                    }
                    return new Token(ETokens.COLON, sr.GetPosition());
                case '=' :
                    sr.Advance();
                    if(sr.GetChar() == '>')
                    {
                        sr.Advance();
                        return new Token(ETokens.ARROW, sr.GetPosition());
                    }
                    return new Token(ETokens.EQUAL, sr.GetPosition());
                case '|' : sr.Advance(); return new Token(ETokens.OR, sr.GetPosition());
                case '&' : sr.Advance(); return new Token(ETokens.AND, sr.GetPosition());
                case '^' : sr.Advance(); return new Token(ETokens.XOR, sr.GetPosition());
                case '<' :
                    sr.Advance();
                    if(sr.GetChar() == '=')
                    {
                        sr.Advance(); return new Token(ETokens.LESSEQUAL, sr.GetPosition());
                    }
                    return new Token(ETokens.LESS, sr.GetPosition());
                case '>' :
                    sr.Advance();
                    if(sr.GetChar() == '=')
                    {
                        sr.Advance(); return new Token(ETokens.GREATEREQUAL, sr.GetPosition());
                    }
                    return new Token(ETokens.GREATER, sr.GetPosition());
                case '.' : 
                    sr.Advance();
                    if(sr.GetChar() == '.')
                    {
                        sr.Advance();
                        return new Token(ETokens.ELLIPSES, sr.GetPosition());
                    }
                    return new Token(ETokens.DOT, sr.GetPosition());
                case '+' : sr.Advance(); return new Token(ETokens.PLUS, sr.GetPosition());
                case '-' : sr.Advance(); return new Token(ETokens.MINUS, sr.GetPosition());
                case '*' : sr.Advance(); return new Token(ETokens.STAR, sr.GetPosition());
                case '/' : 
                    sr.Advance();
                    if(sr.GetChar() == '=')
                    {
                        sr.Advance();
                        return new Token(ETokens.NOTEQUAL, sr.GetPosition());
                    }
                    else if(sr.GetChar() == '/')
                    {
                        sr.Advance();
                        while(sr.GetChar() != '\n' && sr.GetChar() != '\0')
                        {
                            sr.Advance();
                        }
                        sr.Advance();
                        return NextToken(sr);
                    }
                    return new Token(ETokens.SLASH, sr.GetPosition());
                case '(' : sr.Advance(); return new Token(ETokens.LROUND, sr.GetPosition());
                case ')' : sr.Advance(); return new Token(ETokens.RROUND, sr.GetPosition());
                case '[' : sr.Advance(); return new Token(ETokens.LSQUARE, sr.GetPosition());
                case ']' : sr.Advance(); return new Token(ETokens.RSQUARE, sr.GetPosition());
                case '{' : sr.Advance(); return new Token(ETokens.LCURLY, sr.GetPosition());
                case '}' : sr.Advance(); return new Token(ETokens.RCURLY, sr.GetPosition());
                case ' ':
                case '\n':
                case '\t': sr.Advance(); return NextToken(sr);
                case '"' :
                    sr.Advance(); //get first "
                    string val = "";
                    while(sr.GetChar() != '"'){
                        val += sr.GetChar();
                        sr.Advance(); //
                    }
                    sr.Advance(); //get past the "
                    return new Token(ETokens.STRING_VALUE, new List<object>{val}, sr.GetPosition());
                default:
                    if(IsDigit(sr.GetChar()))
                    {
                        string tmp = GetDigits(sr);
                        //maybe double/complex
                        if(sr.GetChar() == '.')
                        {
                            sr.Advance();
                            string tmp2 = GetDigits(sr);
                            if(IsLetter(sr.GetChar()))
                                return new Token(ETokens.ERROR, new List<object>{"Letter after number - identifier can't begin with numbers."}, sr.GetPosition());
                            //check missing suffix
                            if(tmp2.Length == 0)
                                return new Token(ETokens.ERROR, new List<object>{"Mantissa is missing."}, sr.GetPosition());
                            if(sr.GetChar() == 'i')
                            {
                                sr.Advance(); //skips the i
                                string tmp3 = "";
                                if(sr.GetChar() == '-')
                                {
                                    tmp3 = "-";
                                    sr.Advance();
                                }
                                tmp3 += GetDigits(sr);
                                if(IsLetter(sr.GetChar()))
                                    return new Token(ETokens.ERROR, new List<object>{"Letter after number - identifier can't begin with numbers."}, sr.GetPosition());

                                if(tmp3.Length == 0 || tmp3.Length == 1 && tmp3[0] == '-')
                                    return new Token(ETokens.ERROR, new List<object>{"Imaginary part is missing."}, sr.GetPosition());
                                
                                if(sr.GetChar() != '.')
                                    return new Token(ETokens.ERROR, new List<object>{"Mantissa is missing."}, sr.GetPosition());

                                sr.Advance(); //skip .
                                
                                string tmp4 = GetDigits(sr);
                                if(tmp4.Length == 0)
                                    return new Token(ETokens.ERROR, new List<object>{"Mantissa is missing."}, sr.GetPosition());
                                if(IsLetter(sr.GetChar()))
                                    return new Token(ETokens.ERROR, new List<object>{"Letter after number - identifier can't begin with numbers."}, sr.GetPosition());
                                double real = GetDouble(tmp, tmp2);
                                double img = GetDouble(tmp3, tmp4);
                                
                                return new Token(ETokens.COMPLEX_VALUE, new List<object>{real, img}, sr.GetPosition());
                            }
                            return new Token(ETokens.REAL_VALUE, new List<object>{GetDouble(tmp, tmp2)}, sr.GetPosition());
                        }
                        else if(sr.GetChar() == '\\')
                        {
                            //rational
                            sr.Advance(); // \ skip
                            if(sr.GetChar() == '-')
                                return new Token(ETokens.ERROR, new List<object>{"Denominator has to be a positive integer."}, sr.GetPosition());
                            string tmp2 = GetDigits(sr);
                            if(tmp2.Length == 0)
                                return new Token(ETokens.ERROR, new List<object>{"Denominator is missing,"}, sr.GetPosition());
                            if(IsLetter(sr.GetChar()))
                                return new Token(ETokens.ERROR, new List<object>{"Letter after number - identifier can't begin with numbers."}, sr.GetPosition());
                            if(sr.GetChar() == '.')
                                return new Token(ETokens.ERROR, new List<object>{"Denominator has to be an integer number"}, sr.GetPosition());
                            return new Token(ETokens.RATIONAL_VALUE, new List<object>{GetInt(tmp), GetInt(tmp2)}, sr.GetPosition());
                        }
                        else if(IsLetter(sr.GetChar()))
                            return new Token(ETokens.ERROR, new List<object>{"Letter after number - identifier can't begin with numbers."}, sr.GetPosition());
                        return new Token(ETokens.INTEGER_VALUE, new List<object>{GetInt(tmp)}, sr.GetPosition());
                    }
                    //might be letter or not known symbol
                    else if(IsLetter(sr.GetChar()))
                    {
                        string tmp = "";
                        while(IsIdentifierChar(sr.GetChar()))
                        {
                            tmp += sr.GetChar();
                            sr.Advance();
                        }
                        switch(tmp)
                        {
                            case "if": return new Token(ETokens.IF, sr.GetPosition());
                            case "is" : return new Token(ETokens.IS, sr.GetPosition());
                            case "in" : return new Token(ETokens.IN, sr.GetPosition());
                            case "then" : return new Token(ETokens.THEN, sr.GetPosition());
                            case "end" : return new Token(ETokens.END, sr.GetPosition());
                            case "do" : return new Token(ETokens.DO, sr.GetPosition());
                            case "else" : return new Token(ETokens.ELSE, sr.GetPosition());
                            case "integer" : return new Token(ETokens.INTEGER, sr.GetPosition());
                            case "real" : return new Token(ETokens.REAL, sr.GetPosition());
                            case "complex" : return new Token(ETokens.COMPLEX, sr.GetPosition());
                            case "func" : return new Token(ETokens.FUNC, sr.GetPosition());
                            case "print" : return new Token(ETokens.PRINT, sr.GetPosition());
                            case "rational" : return new Token(ETokens.RATIONAL, sr.GetPosition());
                            case "boolean" : return new Token(ETokens.BOOLEAN, sr.GetPosition());
                            case "true" : return new Token(ETokens.BOOLEAN_VALUE, new List<object>{true}, sr.GetPosition());
                            case "false" : return new Token(ETokens.BOOLEAN_VALUE, new List<object>{false}, sr.GetPosition());
                            case "break" : return new Token(ETokens.BREAK, sr.GetPosition());
                            case "continue" : return new Token(ETokens.CONTINUE, sr.GetPosition());
                            case "return" : return new Token(ETokens.RETURN, sr.GetPosition());
                            case "string" : return new Token(ETokens.STRING, sr.GetPosition());
                            case "for": return new Token(ETokens.FOR, sr.GetPosition());
                            case "loop": return new Token(ETokens.LOOP, sr.GetPosition());
                            case "while": return new Token(ETokens.WHILE, sr.GetPosition());
                            default:
                                return new Token(ETokens.ID, new List<object>{tmp}, sr.GetPosition());
                        }
                    }
                    else
                    {
                        throw new System.Exception("Not valid character " + sr.GetChar());
                    }

            }
        }
    }
}
