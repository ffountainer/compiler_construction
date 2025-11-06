using compiler_construction.Semantics;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Keywords;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class StatementNode : TreeNode
{
    private bool meaninglessIf = false;
    
    public override string GetName()
    {
        return "Statement";
    }

    public bool IsMeaningless() => meaninglessIf;
    
    public override void ReadTokens(out Token lastToken)
    {
        Debug.Log($"Starting to parse statement from {firstToken.GetSourceText()} : {firstToken}");

        Token terminator;
        if (firstToken is Var)
        {
            children.Add(NodeFactory.ConstructNode(new DeclarationNode(), lexer, firstToken, out terminator));
        }
        else if (firstToken is If)
        {
            var node = NodeFactory.ConstructNode(new IfNode(), lexer, firstToken, out terminator);
            children.Add(node);

            meaninglessIf = !node.GetEverExecutes();
        }
        else if (firstToken is While)
        {
            children.Add(NodeFactory.ConstructNode(new WhileLoopNode(), lexer, firstToken, out terminator));
        }
        else if (firstToken is For)
        {
            children.Add(NodeFactory.ConstructNode(new ForLoopNode(), lexer, firstToken, out terminator));
        }
        else if (firstToken is Loop)
        {
            children.Add(NodeFactory.ConstructNode(new LoopBodyNode(),  lexer, firstToken, out terminator));
        }
        else if (firstToken is Exit)
        {
            if (IsLoop)
            {
                children.Add(NodeFactory.ConstructNode(new ExitNode(), lexer, firstToken, out terminator));
            }
            else
            {
                throw new SemanticException($"Exit statement can only appear inside loops");
            }
        }
        else if (firstToken is Identifier)
        {
            Debug.Log($"Encountered Assignment Statement, " +
                      $"while first token is {firstToken} [ {firstToken.GetSourceText()} ]");
            
            children.Add(NodeFactory.ConstructNode(new AssignmentNode(), lexer, firstToken, out terminator));
        }
        else if (firstToken is Return)
        {
            if (IsFunc)
            {
                children.Add(NodeFactory.ConstructNode(new ReturnNode(), lexer, firstToken, out terminator));
            }
            else
            {
                throw new SemanticException($"Return statement can only appear inside functions");
            }
        }
        else if (firstToken is Print)
        {
            Debug.Log("Encountered print statement");
            children.Add(NodeFactory.ConstructNode(new PrintNode(), lexer, firstToken, out terminator));
        }
        else if (firstToken is StatementSeparator)
        {
            // Empty statement is skipped
            terminator = firstToken;
        }
        else
        {
            throw new UnexpectedTokenException($"Unexpected token {firstToken}");
        }

        if (terminator is not StatementSeparator)
        {
            throw new UnexpectedTokenException($"Expected end of statement, got {terminator}");
        }
        
        lastToken = lexer.GetNextToken();
        
        Debug.Log($"--- Statement parsed [ {children.LastOrDefault(new ProgramNode()).GetName()} ], " +
                  $"next will start with {lastToken}");
    }
}
