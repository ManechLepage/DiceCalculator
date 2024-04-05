using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionParser : MonoBehaviour
{
    public List<ExpressionManager> expressions = new List<ExpressionManager>();

    string ValueToString(ExpressionManager expr)
    {
        if (!expr.value.Contains("d"))
            return expr.value;
        else
        {
            string text = "[% ";
            /*
            Input : ExpressionManager (ex. expressions = [], value = "26d64", type = ExpressionType.Value)
            Output : string : "[% 26 d64]"
            */

            foreach (char c in expr.value)
            {
                if (c == 'd')
                    text += " d";
                else
                    text += c;
            }

            text += "]";

            return text;
        }
    }

    string Parse(List<ExpressionManager> exprs)
    {
        string text = "";
        
        /*
        Input:
        (Note : the inputs are instances of ExpressionManager class
        Attributes :
        - expressions (List<ExpressionManager>) -- list of sub-expressions
        - value (string) -- string of the expression
        - type (ExpressionType) -- type of the expression -- value, operator, parentesis)

        - expression types
            - value (number)
            - operator
                - addition (+)
                - subtraction (-)
                - multiplication (*)
                - division (/)
                - n dice roll (%) -> n dN
            - parentesis
        
        Output:
        - string
            - Example :
                - Input : "1d6 + 2d4"
                - Output : "[+ [% 1 d6] [% 2 d4]]"
            - Example 2 :
                - Input : "1d8 * (2d6 + 3)"
                - Output : "[* [% 1 d8] [+ [% 2 d6] 3]]"
        
        Brainstorm:
        - hypothetic input : 1d8 * (2d6 + 3)
        - wanted output : [* [% 1 d8] [+ [% 2 d6] 3]]
        text: ""
        */

        foreach (ExpressionManager expr in exprs)
        {
            if (expr.type == ExpressionType.Value)
            {
                text += ValueToString(expr);
            }
            else if (expr.type == ExpressionType.Operator)
            {
                text += "[" + expr.value + " ";
                foreach (ExpressionManager subExpr in expr.expressions)
                {
                    List<ExpressionManager> subExprs = new List<ExpressionManager>();
                    subExprs.Add(subExpr);
                    
                    text += Parse(subExprs) + " ";
                }
                text += "]";
            }
            else if (expr.type == ExpressionType.Parentesis)
            {
                text += "[";
                text += Parse(expr.expressions);
                text += "]";
            }
        }

        return "";
    }
}
