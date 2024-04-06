using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public List<GameObject> expressions = new List<GameObject>(); 
    public GameObject operatorPrefab; 

    public void Update()
    {
        WidthManager();
    }

    public void UpdatePanel()
    {
        expressions.Clear();
        foreach (Transform child in transform)
        {
            expressions.Add(child.gameObject);
        }
        AddOperators();
        WidthManager();
    }

    public void AddOperators()
    {
        bool isLastOperator = true;
        foreach (GameObject expression in expressions)
        {
            if (expression.GetComponent<ExpressionManager>() == null) continue;
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

    public void WidthManager()
    {
        float width = 30;
        foreach (Transform expression in transform)
        {
            width += expression.GetComponent<RectTransform>().rect.width;
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, GetComponent<RectTransform>().rect.height);
        if (transform.parent.GetComponent<PanelManager>() != null)
        {
            transform.parent.GetComponent<PanelManager>().UpdatePanel();
        }
    }
}
