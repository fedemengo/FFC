cat FParser/Parser.cs | tr '\n' '^' | tr '\r' '~' | sed -e 's/internal enum ETokens {.*[0-9]};~^~^//' | tr '^' '\n' | tr '~' '\r' | sed -e 's/error/ERROR/'
