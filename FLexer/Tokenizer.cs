using System;
using System.Collections.Generic;
using FFC.FGen;

namespace FFC.FLexer
{
    class Tokenizer
    {
        private Token next = null;
        private bool blank = true;
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
        static bool IsIdentifierChar(char c) => Char.IsLetterOrDigit(c) || c == '_';

        static double GetDouble(string before, string after) => double.Parse(before + "." + after);
        static int GetInt(string num) => int.Parse(num);
        static string GetDigits(SourceReader sr)
        {
            string tmp = "";
            while(Char.IsDigit(sr.GetChar()))
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
            while((current = NextToken(sr)).Type != ETokens.EOF)
            {
                ans.Add(current);
                if(current.Type == ETokens.ERROR)
                    sr.SkipBlank();
            }
            ans.Add(current);
            return ans;
        }

        public Token NextToken(SourceReader sr)
        {
            Token t;
            if(next != null)
            {
                t = next;
                next = null;
            }
            else
                t = _NextToken(sr);
            // to avoid blank before ellipsis
            if(t.Type == ETokens.ELLIPSIS && (blank || IsBlank(sr.GetChar())))
                t.Type = ETokens.ERROR;
            else
                blank = false;
            return t;
        }
        private Token ParseComplex(string num1, string frac1, SourceReader sr, Position begin)
        {
            sr.Advance(); //skips the i
            string num2 = "";
            if(sr.GetChar() == '-')
            {
                num2 = "-";
                sr.Advance();
            }
            num2 += GetDigits(sr);
            if(Char.IsLetter(sr.GetChar()))
                return new Token(ETokens.ERROR, new List<object>{"Letter after number - identifier can't begin with numbers."}, begin, sr.GetPosition());

            if(num2.Length == 0 || num2.Length == 1 && num2[0] == '-')
                return new Token(ETokens.ERROR, new List<object>{"Imaginary part is missing."}, begin, sr.GetPosition());

            string frac2 = "0";
            if(sr.GetChar() == '.')
            {
                sr.Advance(); //skip .
                frac2 = GetDigits(sr);
                if(frac2.Length == 0)
                    return new Token(ETokens.ERROR, new List<object>{"Mantissa is missing."}, begin, sr.GetPosition());
            }
            if(Char.IsLetter(sr.GetChar()))
                return new Token(ETokens.ERROR, new List<object>{"Letter after number - identifier can't begin with numbers."}, begin, sr.GetPosition());

            double real = GetDouble(num1, frac1);
            double img = GetDouble(num2, frac2);
            return new Token(ETokens.COMPLEX_VALUE, new List<object>{real, img}, begin, sr.GetPosition());
        }
        private Token _NextToken(SourceReader sr)
        {
            Position begin = sr.GetPosition();
            if(sr.Empty()) return new Token(ETokens.EOF, begin, sr.GetPosition());
            switch(sr.GetChar())
            {
                case ',' : sr.Advance(); return new Token(ETokens.COMMA, begin, sr.GetPosition());
                case ';' : sr.Advance(); return new Token(ETokens.SEMICOLON, begin, sr.GetPosition());
                case ':' :
                    sr.Advance();
                    if(sr.GetChar() == '=')
                    {
                        sr.Advance();
                        return new Token(ETokens.ASSIGN, begin, sr.GetPosition());
                    }
                    return new Token(ETokens.COLON, begin, sr.GetPosition());
                case '=' :
                    sr.Advance();
                    if(sr.GetChar() == '>')
                    {
                        sr.Advance();
                        return new Token(ETokens.ARROW, begin, sr.GetPosition());
                    }
                    return new Token(ETokens.EQUAL, begin, sr.GetPosition());
                case '|' : sr.Advance(); return new Token(ETokens.OR, begin, sr.GetPosition());
                case '&' : sr.Advance(); return new Token(ETokens.AND, begin, sr.GetPosition());
                case '^' : sr.Advance(); return new Token(ETokens.XOR, begin, sr.GetPosition());
                case '!' : sr.Advance(); return new Token(ETokens.NOT, begin, sr.GetPosition());
                case '<' :
                    sr.Advance();
                    if(sr.GetChar() == '=')
                    {
                        sr.Advance(); return new Token(ETokens.LESSEQUAL, begin, sr.GetPosition());
                    }
                    return new Token(ETokens.LESS, begin, sr.GetPosition());
                case '>' :
                    sr.Advance();
                    if(sr.GetChar() == '=')
                    {
                        sr.Advance(); return new Token(ETokens.GREATEREQUAL, begin, sr.GetPosition());
                    }
                    return new Token(ETokens.GREATER, begin, sr.GetPosition());
                case '.' : 
                    sr.Advance();
                    if(sr.GetChar() == '.')
                    {
                        sr.Advance();
                        return new Token(ETokens.ELLIPSIS, begin, sr.GetPosition());
                    }
                    //we have either a name or an integer
                    Position p = sr.GetPosition();
                    string num = GetDigits(sr);
                    if(num.Length > 0) next = new Token(ETokens.INTEGER_VALUE, new List<object>{int.Parse(num)}, p, sr.GetPosition());
                    return new Token(ETokens.DOT, begin, p);
                case '+' : sr.Advance(); return new Token(ETokens.PLUS, begin, sr.GetPosition());
                case '-' : sr.Advance(); return new Token(ETokens.MINUS, begin, sr.GetPosition());
                case '*' : sr.Advance(); return new Token(ETokens.STAR, begin, sr.GetPosition());
                case '%' : sr.Advance(); return new Token(ETokens.MODULO, begin, sr.GetPosition());
                case '/' : 
                    sr.Advance();
                    if(sr.GetChar() == '=')
                    {
                        sr.Advance();
                        return new Token(ETokens.NOTEQUAL, begin, sr.GetPosition());
                    }
                    else if(sr.GetChar() == '/')
                    {
                        char prev = '/';
                        sr.Advance();
                        while(!(sr.GetChar() == '\0' || sr.GetChar() == '\n' && prev != '\\'))
                        {
                            prev = sr.GetChar();                               
                            sr.Advance();
                        }
                        sr.Advance();
                        return NextToken(sr);
                    }
                    else if(sr.GetChar() == '*')
                    {
                        //Multi line comments
                        sr.Advance();
                        char prev = sr.GetChar(); sr.Advance();
                        while(prev != '*' || sr.GetChar() != '/')
                        {
                            prev = sr.GetChar();
                            sr.Advance();
                        }
                        sr.Advance(); //Skip the last slash
                        //skips the comment
                        return NextToken(sr);
                    }
                    return new Token(ETokens.SLASH, begin, sr.GetPosition());
                case '(' : sr.Advance(); return new Token(ETokens.LROUND, begin, sr.GetPosition());
                case ')' : sr.Advance(); return new Token(ETokens.RROUND, begin, sr.GetPosition());
                case '[' : sr.Advance(); return new Token(ETokens.LSQUARE, begin, sr.GetPosition());
                case ']' : sr.Advance(); return new Token(ETokens.RSQUARE, begin, sr.GetPosition());
                case '{' : sr.Advance(); return new Token(ETokens.LCURLY, begin, sr.GetPosition());
                case '}' : sr.Advance(); return new Token(ETokens.RCURLY, begin, sr.GetPosition());
                case ' ':
                case '\n':
                case '\t': blank = true; sr.Advance(); return NextToken(sr);
                case '"' :
                    sr.Advance(); //get first "
                    string val = "";
                    while(sr.GetChar() != '"'){
                        //escape character
                        if(sr.GetChar() == '\\')
                        {
                            sr.Advance();
                            switch(sr.GetChar())
                            {
                                case '"':
                                case '\\':
                                    val += sr.GetChar();
                                    break;
                                case 'n':
                                    val += '\n';
                                    break;
                                case 't':
                                    val += '\t';
                                    break;
                                default : //non recognized escape characters
                                    //just ignored
                                    break;
                            }
                        }
                        else
                        {
                            val += sr.GetChar();
                        }
                        sr.Advance(); //moves to next char
                    }
                    sr.Advance(); //get past the "
                    return new Token(ETokens.STRING_VALUE, new List<object>{val}, begin, sr.GetPosition());
                default:
                    if(Char.IsDigit(sr.GetChar()))
                    {
                        string tmp = GetDigits(sr);
                        //maybe double/complex
                        if(sr.GetChar() == '.')
                        {
                            sr.Advance();
                            if(sr.GetChar() == '.')
                            {
                                sr.Advance();
                                Position pos = sr.GetPosition();
                                next = new Token(ETokens.ELLIPSIS, new Position(pos.Row, pos.Column-2), pos);
                                return new Token(ETokens.INTEGER_VALUE, new List<object>{int.Parse(tmp)}, begin, new Position(pos.Row, pos.Column-2));
                            }
                            string tmp2 = GetDigits(sr);
                            //check missing suffix
                            if(tmp2.Length == 0)
                                return new Token(ETokens.ERROR, new List<object>{"Mantissa is missing."}, begin, sr.GetPosition());
                            //complex values
                            if(sr.GetChar() == 'i')
                                return ParseComplex(tmp, tmp2, sr, begin);
                            //can be checked only after i
                            if(Char.IsLetter(sr.GetChar()))
                                return new Token(ETokens.ERROR, new List<object>{"Letter after number - identifier can't begin with numbers."}, begin, sr.GetPosition());
                            return new Token(ETokens.REAL_VALUE, new List<object>{GetDouble(tmp, tmp2)}, begin, sr.GetPosition());
                        }
                        else if(sr.GetChar() == '\\')
                        {
                            //rational
                            sr.Advance(); // \ skip
                            if(sr.GetChar() == '-')
                                return new Token(ETokens.ERROR, new List<object>{"Denominator has to be a positive integer."}, begin, sr.GetPosition());
                            string tmp2 = GetDigits(sr);
                            if(tmp2.Length == 0)
                                return new Token(ETokens.ERROR, new List<object>{"Denominator is missing,"}, begin, sr.GetPosition());
                            if(Char.IsLetter(sr.GetChar()))
                                return new Token(ETokens.ERROR, new List<object>{"Letter after number - identifier can't begin with numbers."}, begin, sr.GetPosition());
                            if(sr.GetChar() == '.')
                                return new Token(ETokens.ERROR, new List<object>{"Denominator has to be an integer number"}, begin, sr.GetPosition());
                            return new Token(ETokens.RATIONAL_VALUE, new List<object>{GetInt(tmp), GetInt(tmp2)}, begin, sr.GetPosition());
                        }
                        else if(sr.GetChar() == 'i')
                        {
                            return ParseComplex(tmp, "0", sr, begin);
                        }
                        else if(Char.IsLetter(sr.GetChar()))
                        {
                            return new Token(ETokens.ERROR, new List<object>{"Letter after number - identifier can't begin with numbers."}, begin, sr.GetPosition());
                        }
                        return new Token(ETokens.INTEGER_VALUE, new List<object>{GetInt(tmp)}, begin, sr.GetPosition());
                    }
                    //might be letter or not known symbol
                    else if(Char.IsLetter(sr.GetChar()))
                    {
                        string tmp = "";
                        while(IsIdentifierChar(sr.GetChar()))
                        {
                            tmp += sr.GetChar();
                            sr.Advance();
                        }
                        switch(tmp)
                        {
                            case "if": return new Token(ETokens.IF, begin, sr.GetPosition());
                            case "is" : return new Token(ETokens.IS, begin, sr.GetPosition());
                            case "in" : return new Token(ETokens.IN, begin, sr.GetPosition());
                            case "then" : return new Token(ETokens.THEN, begin, sr.GetPosition());
                            case "end" : return new Token(ETokens.END, begin, sr.GetPosition());
                            case "do" : return new Token(ETokens.DO, begin, sr.GetPosition());
                            case "else" : return new Token(ETokens.ELSE, begin, sr.GetPosition());
                            case "integer" : return new Token(ETokens.INTEGER, begin, sr.GetPosition());
                            case "real" : return new Token(ETokens.REAL, begin, sr.GetPosition());
                            case "complex" : return new Token(ETokens.COMPLEX, begin, sr.GetPosition());
                            case "func" : return new Token(ETokens.FUNC, begin, sr.GetPosition());
                            case "print" : return new Token(ETokens.PRINT, begin, sr.GetPosition());
                            case "read" : return new Token(ETokens.READ, begin, sr.GetPosition());
                            case "rational" : return new Token(ETokens.RATIONAL, begin, sr.GetPosition());
                            case "boolean" : return new Token(ETokens.BOOLEAN, begin, sr.GetPosition());
                            case "true" : return new Token(ETokens.BOOLEAN_VALUE, new List<object>{true}, begin, sr.GetPosition());
                            case "false" : return new Token(ETokens.BOOLEAN_VALUE, new List<object>{false}, begin, sr.GetPosition());
                            case "break" : return new Token(ETokens.BREAK, begin, sr.GetPosition());
                            case "continue" : return new Token(ETokens.CONTINUE, begin, sr.GetPosition());
                            case "return" : return new Token(ETokens.RETURN, begin, sr.GetPosition());
                            case "string" : return new Token(ETokens.STRING, begin, sr.GetPosition());
                            case "for": return new Token(ETokens.FOR, begin, sr.GetPosition());
                            case "loop": return new Token(ETokens.LOOP, begin, sr.GetPosition());
                            case "while": return new Token(ETokens.WHILE, begin, sr.GetPosition());
                            default:
                                return new Token(ETokens.ID, new List<object>{tmp}, begin, sr.GetPosition());
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
