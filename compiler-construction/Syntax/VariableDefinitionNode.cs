using System.Collections;
using compiler_construction.Semantics;
using compiler_construction.Tokenization;
using compiler_construction.Tokenization.Symbols;

namespace compiler_construction.Syntax;

public class VariableDefinitionNode : TreeNode
{
    public override string GetName()
    {
        return "VariableDefinition";
    }

    public override void ReadTokens(out Token lastToken)
    {
        if (firstToken is not Identifier)
        {
            throw new UnexpectedTokenException("Expected identifier, but got " + firstToken);
        }
        
        foreach (DictionaryEntry entry in SyntaxAnalyzer.GetCurrentScope().GetScope())
        {
            Debug.Log($"Key: {entry.Key}, Value: {entry.Value}");
        }
        
        if (SyntaxAnalyzer.GetCurrentScope().GetScope().ContainsKey(firstToken.GetSourceText()))
        {
            throw new SemanticException($"Identifier \"{firstToken.GetSourceText()}\" is already declared in this scope");
        } 
        
        children.Add(NodeFactory.ConstructNode(new IdentifierNode(), lexer, firstToken));

        bool isDefined = false;
        
        lastToken = lexer.GetNextToken();
        if (lastToken is ColonEqual)
        {
            isDefined = true;
            SyntaxAnalyzer.AddToCurScope(firstToken.GetSourceText(), isDefined);
            Debug.Log("Got colon equal in var def, construct expr");
            children.Add(NodeFactory.ConstructNode(new ExpressionNode(), lexer, lexer.GetNextToken(), out lastToken));
            Debug.Log($"Var def got {lastToken.GetSourceText()} as last token out of expression");
        }
        else
        {
            SyntaxAnalyzer.AddToCurScope(firstToken.GetSourceText(), isDefined);
        }

    }
}