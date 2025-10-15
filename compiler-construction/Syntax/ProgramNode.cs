using compiler_construction.Tokenization;

namespace compiler_construction.Syntax;

public class ProgramNode : TreeNode
{
    public override string GetName() => "Program";

    override public void ReadTokens()
    {
        var token = lexer.GetNextToken();
        while (token is not FinishProgram)
        {
            var statement = new StatementNode();
            statement.Init(lexer, token);
            statement.ReadTokens();
            children.Add(statement);
            
            token = lexer.GetNextToken();
        }
    }
}
