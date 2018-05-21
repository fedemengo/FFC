namespace naiveLex
{
    public enum TokenType
    {
        IS,
        IF,
        THEN,
        ELSE,
        END,
        WHILE,
        FOR,
        IN,
        LOOP,
        BREAK,
        FUNC,
        DO,
        RETURN,
        PRINT,
        INTEGER,
        COMPLEX,
        RATIONAL,
        REAL,
        STRING,
        BOOLEAN,
        ASSIGN,
        ARROW,
        DOT,
        COMMA,
        COLON,
        SEMICOLON,
        PLUS,
        MINUS,
        STAR,
        SLASH,
        LESS,
        LESSEQUAL,
        GREATER,
        GREATEREQUAL,
        EQUAL,
        NOTEQUAL,
        AND,
        OR,
        XOR,
        LROUND,
        RROUND,
        LSQUARE,
        RSQUARE,
        LCURLY,
        RCURLY,
        BOOLEAN_VALUE,        // single value, bool
        INTEGER_VALUE,        // single value, number
        REAL_VALUE,           // single value, number
        RATIONAL_VALUE,       // pair of value, number
        COMPLEX_VALUE,        // pair of value, number   
        STRING_VALUE,         // single value, string
        ID                    // single value, string
    }
}