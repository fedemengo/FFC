perform_task() {
    arg=1
    [[ ( "$1" =~ ^"compile"$|^"run"$ ) ]] && arg=2
    
    if [[ ( "$#" -lt "$arg" ) ]]; then
        echo "Too few argument"
        return 1
    elif [[ ( "$#" -gt "$arg" ) ]]; then 
        echo "Too many arguments"
        return 1
    fi
    case $1 in
    "help")
        echo ""
        echo "parser - Generate Parser for grammar"
        echo "build - Build project"
        echo "lib - Generate FFC.dll"
        echo "compile FILE - Compile the source FILE"
        echo "run EXE - Run the executable EXEC"
        return 1
        ;;
    "build")
        echo "Building the project..."
        echo ""
        msbuild
        echo ""
        return 0
        ;;
    "compile")
        echo "Compiling..."
        echo ""
        mono bin/Release/FFC.exe "$2"
        echo ""
        return 0
        ;;
    "run")
        echo ""
        mono "$2"
        echo ""
        return 0
        ;;
    "lib")
        echo "Generating library..."
        echo ""
        csc FRunTime/*.cs /t:library /out:FFC.dll
        echo ""
        return 0
        ;;
    "parser")
        echo "Generating parser..."
        echo ""
        # generate original parser
        mono FParser/gppg/gppg.exe FParser/gppg/grammar.y > FParser/Parser.cs

        echo "Cleaning up..."
        # Refactors error to ERROR
        cat FParser/Parser.cs | sed -e 's/error/ERROR/' > tmp.parser
        # generate ETokens
        echo "using System;" > FLexer/ETokens.cs
        echo "" >> FLexer/ETokens.cs
        echo "namespace FFC.FLexer" >> FLexer/ETokens.cs
        echo "{" >> FLexer/ETokens.cs
        # matches the tokens and prints them
        awk '/[A-Z_]*=[0-9]*,/{print $0}' tmp.parser >> FLexer/ETokens.cs
        # finishes the file
        echo "}" >> FLexer/ETokens.cs
        # remove tokens from tmp.parser and prints in Parser.cs
        cat tmp.parser | tr '\n' '^' | tr '\r' '~' | sed -e 's/internal enum ETokens {.*[0-9]};~^~^//' | tr '^' '\n' | tr '~' '\r' > FParser/Parser.cs
        # remove tmp.parser
        rm tmp.parser
        echo ""
        return 0
        ;;
    *)
        echo "Unkwon argument"
        echo "Use 'help' for more information"
        return 1
        ;;
    esac
}

perform_task "$@"

if [ "$?" == "0" ]; then
    echo "Task completed!"
fi
