using compiler_construction.Syntax;
using compiler_construction.Syntax.Literals;

namespace compiler_construction.Interpretation;

public class AssignmentInterpreter : Interpretable
{
    private AssignmentNode _assignment;

    public AssignmentInterpreter(AssignmentNode assignment)
    {
        _assignment = assignment;
        children = assignment.GetChildren();
    }

    public override void Interpret()
    {
        ReferenceNode reference = (ReferenceNode)_assignment.GetChildren().First();
        ExpressionNode expression = (ExpressionNode)_assignment.GetChildren().Last();
        Debug.Log($"Im interpreting assignment for reference {reference.GetIdentifier().GetValue()}");
        ExpressionInterpreter expressionInterpreter = new ExpressionInterpreter(expression);
        expressionInterpreter.Interpret();
        ExpressionNode calculatedExpression = ConstructExpressionFromExprInterpreter(expressionInterpreter);
        if (reference.getWhatReference() is WhatReference.Ident)
        {
            Debug.Log("The reference is by ident");
            InheritValues(expressionInterpreter, "Error inheriting from the expression");
            SetIdentifier(reference.GetIdentifier(), calculatedExpression);
        }
        // example:
        // var tuple := {a := 3, b, 4}
        // tuple.a := 5
        else if (reference.getWhatReference() is WhatReference.Tuple)
        {
            Debug.Log("The reference is by tuple access");
            // tuple
            IdentifierNode tupleName = (IdentifierNode)reference.GetChildren().First();
            Debug.Log($"Trying to assign to tuple with name {tupleName.GetValue()}");
            if (FindExpression(tupleName) == null)
            {
                throw new InterpretationException($"Interpreter: no tuple called {tupleName.GetValue()} is found");
            }

            ExpressionNode tupleExpression = FindExpression(tupleName);
            ExpressionInterpreter interpreter = new ExpressionInterpreter(tupleExpression);
            Debug.Log("Now i will interpret an expression for the tupleValue in Identifiers list");
            interpreter.Interpret();
            if (interpreter.GetWhatExpression() != WhatExpression.TupleExpr)
            {
                throw new InterpretationException($"Interpreter: {interpreter.GetWhatExpression()} cannot be referenced as a tuple");
            }
            List<TupleElementNode> initialTuple = interpreter.GetTupleValue();
            List<TupleElementNode> newTuple = new List<TupleElementNode>();
            List<TreeNode> newChildren = new List<TreeNode>();
            // tupleElement has one child: either an int index or ident
            TupleAccessNode tupleElement = (TupleAccessNode)reference.GetChildren().Skip(1).First();
            if (tupleElement.GetWhatTupleReference() is WhatTupleReference.TupleByIdent)
            {
                IdentifierNode tupleElementName = (IdentifierNode)tupleElement.GetChildren().First();
                Debug.Log($"The tuple reference is by ident + {tupleElementName.GetValue()}");
                bool found = false;
                
                foreach (TupleElementNode element in initialTuple)
                {
                    
                    if (element.key != null && element.key.GetValue() == tupleElementName.GetValue())
                    {
                        found = true;
                        newTuple.Add(new TupleElementNode(element.key, calculatedExpression, element.GetChildren()));
                    }
                    else
                    {
                        newTuple.Add(new TupleElementNode(element.key, element.value, element.GetChildren()));
                    }
                }
                if (!found)
                {
                    throw new InterpretationException($"Interpreter: no element with key {tupleElementName.GetValue()} found in tuple {tupleName.GetValue()}");
                }

                TreeNode node = new TupleNode();
                
                foreach (TupleElementNode element in newTuple)
                {
                    node.AddChild(element);
                }
                node = new LiteralNode().AddChild(node);
                node = new PrimaryNode().AddChild(node);
                node = new UnaryNode().AddChild(node);
                node = new TermNode().AddChild(node);
                node = new FactorNode().AddChild(node);
                node = new RelationNode().AddChild(node);
                
                newChildren.Add(node);
                
                SetIdentifier(reference.GetIdentifier(), (new ExpressionNode(newTuple, newChildren)));
            }

            if (tupleElement.GetWhatTupleReference() is WhatTupleReference.TupleByIndex)
            {
                Debug.Log("The tuple reference is by index");
                IntegerLiteral tupleIndex = (IntegerLiteral)tupleElement.GetChildren().First();
                int index = tupleIndex.Value - 1;
                if (initialTuple.Count < index)
                {
                    throw new InterpretationException("Interpreter: invalid tuple index");
                }

                for (int i = 0; i < initialTuple.Count; i++)
                {
                    if (i == index)
                    {
                        newTuple.Add(new TupleElementNode(initialTuple[i].key, calculatedExpression, initialTuple.ElementAt(i).GetChildren()));
                    }
                    else
                    {
                        newTuple.Add(new TupleElementNode(initialTuple[i].key, initialTuple[i].value, initialTuple.ElementAt(i).GetChildren()));
                    }
                }
                
                TreeNode node = new TupleNode();
                
                foreach (TupleElementNode element in newTuple)
                {
                    node.AddChild(element);
                }
                node = new LiteralNode().AddChild(node);
                node = new PrimaryNode().AddChild(node);
                node = new UnaryNode().AddChild(node);
                node = new TermNode().AddChild(node);
                node = new FactorNode().AddChild(node);
                node = new RelationNode().AddChild(node);
                
                newChildren.Add(node);
                
                SetIdentifier(reference.GetIdentifier(), (new ExpressionNode(newTuple, newChildren)));
            }
        }
        else if (reference.getWhatReference() is WhatReference.Array)
        {
            IdentifierNode arrayName = (IdentifierNode)reference.GetChildren().First();
            if (FindExpression(arrayName) == null)
            {
                throw new InterpretationException($"Interpreter: no array called {arrayName.GetValue()} is found");
            }

            ExpressionNode arrayExpression = FindExpression(arrayName);
            ExpressionInterpreter interpreter = new ExpressionInterpreter(arrayExpression);
            List<TreeNode> newChildren = new List<TreeNode>();
            interpreter.Interpret();
            if (interpreter.GetWhatExpression() != WhatExpression.ArrayExpr)
            {
                throw new InterpretationException($"Interpreter: {interpreter.GetWhatExpression()} cannot be referenced as an array");
            }
            List<ExpressionNode> initialArray = interpreter.GetArrayValue();
            List<ExpressionNode> newArray = new List<ExpressionNode>();
            
            ArrayAccessNode arrayElement = (ArrayAccessNode)reference.GetChildren().Skip(1).First();
            ExpressionNode arrayIndexExpr = (ExpressionNode)arrayElement.GetChildren().First();
            ExpressionInterpreter arrayIndexInterpreter = new ExpressionInterpreter(arrayIndexExpr);
            arrayIndexInterpreter.Interpret();
            if (arrayIndexInterpreter.GetWhatExpression() != WhatExpression.IntegerExpr)
            {
                throw new InterpretationException("Interpreter: invalid array index");
            }

            int index = arrayIndexInterpreter.GetIntValue() - 1;
            
            if (initialArray.Count <= index)
            {
                int initialCount = initialArray.Count;
                for (int i = 0; i < (index - initialCount + 1); i++)
                {
                    initialArray.Add(ConstructNullExprArray());
                }
            }

            for (int i = 0; i < initialArray.Count; i++)
            {
                if (i == index)
                {
                    newArray.Add(calculatedExpression);
                }
                else
                {
                    newArray.Add(initialArray[i]);
                }
            }
            
            TreeNode node = new ArrayNode();
                
            foreach (ExpressionNode element in newArray)
            {
                node.AddChild(element);
            }
            node = new LiteralNode().AddChild(node);
            node = new PrimaryNode().AddChild(node);
            node = new UnaryNode().AddChild(node);
            node = new TermNode().AddChild(node);
            node = new FactorNode().AddChild(node);
            node = new RelationNode().AddChild(node);
                
            newChildren.Add(node);
            
            SetIdentifier(reference.GetIdentifier(), (new ExpressionNode(newArray,  newChildren)));
        }
    }
}