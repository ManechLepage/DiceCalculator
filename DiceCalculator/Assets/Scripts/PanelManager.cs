using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public List<GameObject> expressions = new List<GameObject>(); 
    public GameObject operatorPrefab; 

    public void UpdatePanel()
    {
        expressions.Clear();
        foreach (Transform child in transform)
        {
            expressions.Add(child.gameObject);
        }
        AddOperators();
    }

    public void AddOperators()
    {
        bool isLastOperator = true;
        foreach (GameObject expression in expressions)
        {
            if (expression.GetComponent<ExpressionManager>().type == ExpressionType.Value || expression.GetComponent<ExpressionManager>().type == ExpressionType.Parentesis)
            {
                if (!isLastOperator)
                {
                    GameObject operatorObject = Instantiate(operatorPrefab, transform);
                    operatorObject.transform.SetSiblingIndex(expressions.IndexOf(expression));
                    expressions.Insert(expressions.IndexOf(expression), operatorObject);
                }
                isLastOperator = false;
            }
            else if (expression.GetComponent<ExpressionManager>().type == ExpressionType.Operator)
            {
                isLastOperator = true;
            }
        }
    }
}
