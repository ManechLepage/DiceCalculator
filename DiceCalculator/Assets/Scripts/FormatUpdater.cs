using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormatUpdater : MonoBehaviour
{
    public GameObject baseBracket;
    
    public void UpdateFormat()
    {
        UpdateExpression(baseBracket);
    }
    
    void UpdateExpression(GameObject expressionObject)
    {
        List<GameObject> children = new List<GameObject>();
        ExpressionManager expression = expressionObject.GetComponent<ExpressionManager>();

        foreach (Transform child in expressionObject.transform)
        {
            GameObject childObject = child.gameObject;
            
            if (childObject.GetComponent<ExpressionManager>() != null)
                children.Add(childObject);
        }

        expression.expressions = new List<ExpressionManager>();
        
        bool hasOperators = false;
        int i = 0;
        foreach (GameObject child in children)
        {
            if (child != expressionObject)
            {
                ExpressionManager expr = child.GetComponent<ExpressionManager>();

                if (expr != null)
                {
                    if (expr.type == ExpressionType.Operator || expr.type == ExpressionType.MinBracket || expr.type == ExpressionType.MaxBracket)
                    {
                        expr.expressions = new List<ExpressionManager>();

                        ExpressionManager e1;
                        ExpressionManager e2;
                        if (expr.type == ExpressionType.Operator)
                        {
                            e1 = children[i - 1].GetComponent<ExpressionManager>();
                            e2 = children[i + 1].GetComponent<ExpressionManager>();
                        }
                        else
                        {
                            e1 = children[i + 1].GetComponent<ExpressionManager>();
                            e2 = children[i + 2].GetComponent<ExpressionManager>();
                        }

                        expr.expressions.Add(e1);
                        expr.expressions.Add(e2);

                        hasOperators = true;

                        expression.expressions.Add(expr);
                    }
                    else if (expr.type == ExpressionType.Parentesis)
                    {
                        UpdateExpression(child);
                    }
                }
            }
            i += 1;
        }

        if (!hasOperators)
        {
            expression.expressions = new List<ExpressionManager>();
            ExpressionManager expr;
            
            try
            {
                expr = children[0].GetComponent<ExpressionManager>();
                expression.expressions.Add(expr);
            }
            catch
            {}
        }
    }
}
