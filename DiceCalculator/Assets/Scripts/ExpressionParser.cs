using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionParser : MonoBehaviour
{
    public List<ExpressionManager> expressions = new List<ExpressionManager>();

    void TestInitialize()
    {
        // 1d8 * (2d6 + 3)
        ExpressionManager expr1 = new ExpressionManager();
        expr1.value = "1d8";
        expr1.type = ExpressionType.Value;

        ExpressionManager expr2 = new ExpressionManager();
        expr2.value = "2d6";
        expr2.type = ExpressionType.Value;

        ExpressionManager expr3 = new ExpressionManager();
        expr3.value = "3";
        expr3.type = ExpressionType.Value;

        ExpressionManager expr4 = new ExpressionManager();
        expr4.value = "+";
        expr4.type = ExpressionType.Operator;
        expr4.expressions.Add(expr2);
        expr4.expressions.Add(expr3);

        ExpressionManager expr5 = new ExpressionManager();
        expr5.value = "";
        expr5.type = ExpressionType.Parentesis;
        expr5.expressions.Add(expr4);

        ExpressionManager expr6 = new ExpressionManager();
        expr6.value = "*";
        expr6.type = ExpressionType.Operator;
        expr6.expressions.Add(expr1);
        expr6.expressions.Add(expr5);

        expressions.Add(expr6);
    }

    void Start()
    {
        Debug.Log("Expression Parser");
        TestInitialize();
        string text = Parse(expressions);
        Debug.Log(text);
    }

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
                text += ValueToString(expr);
            
            else if (expr.type == ExpressionType.Operator)
            {
                text += "[" + expr.value;
                foreach (ExpressionManager subExpr in expr.expressions)
                {
                    List<ExpressionManager> subExprs = new List<ExpressionManager>();
                    subExprs.Add(subExpr);
                    
                    text += " " + Parse(subExprs);
                }
                text += "]";
            }

            else if (expr.type == ExpressionType.Parentesis)
                text += Parse(expr.expressions);
        }

        return text;
    }
}
