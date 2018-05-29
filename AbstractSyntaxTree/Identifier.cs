namespace AbstractSyntaxTree
{
    public class Identifier
    {
        public string Name {get; set;}
        public bool IsUndifined() => Name == string.Empty;
        Identifier()
        {
            Name = string.Empty;
        }
        Identifier(string s)
        {
            Name = s;
        }
    }
}