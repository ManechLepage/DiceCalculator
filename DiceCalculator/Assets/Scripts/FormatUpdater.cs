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

        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            children.Add(child.gameObject);
        }

        expression.expressions = new List<ExpressionManager>();
        
        bool hasOperators = false;
        int i = 0;
        foreach (GameObject child in children)
        {
            ExpressionManager expr = child.GetComponent<ExpressionManager>();

            Debug.Log($"{child.name}");

            if (expr != null)
            {
                if (expr.type == ExpressionType.Operator)
                {
                    expr.expressions = new List<ExpressionManager>();
                    expr.expressions.Add(children[i - 1].GetComponent<ExpressionManager>());
                    expr.expressions.Add(children[i + 1].GetComponent<ExpressionManager>());
                    hasOperators = true;

                    expression.expressions.Add(expr);
                }
                else if (expr.type == ExpressionType.Parentesis)
                {
                    UpdateExpression(child);
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
