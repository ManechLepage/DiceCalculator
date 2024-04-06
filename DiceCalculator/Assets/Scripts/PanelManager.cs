using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public List<GameObject> expressions = new List<GameObject>(); 
    public GameObject operatorPrefab; 
    public int maxLength = -1;
    public bool hasOperators = true;

    public void Update()
    {
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        if (hasOperators) 
        {
            AddOperators();
        }
        WidthManager();
        HandleMaxLength();
    }

    public void AddOperators()
    {
        bool isLastOperator = true;
        List<int> operatorsToAdd = new List<int>();
        foreach (Transform child in transform)
        {
            GameObject expression = child.gameObject;
            if (expression.GetComponent<ExpressionManager>() == null) continue;

            if (expression.GetComponent<ExpressionManager>().type != ExpressionType.Operator)
            {
                if (!isLastOperator)
                {
                    operatorsToAdd.Add(child.GetSiblingIndex());
 
                }
                isLastOperator = false;
            }
            else
            {
                isLastOperator = true;
            }
        }
        for (int i = 0; i < operatorsToAdd.Count; i++)
        {
            GameObject operatorObject = Instantiate(operatorPrefab, transform);
            operatorObject.transform.SetSiblingIndex(operatorsToAdd[i]);
        }
    }

    public void WidthManager()
    {
        float width = 5;
        foreach (Transform expression in transform)
        {
            width += expression.GetComponent<RectTransform>().rect.width + 10f;
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, GetComponent<RectTransform>().rect.height);
        if (transform.parent.GetComponent<PanelManager>() != null)
        {
            transform.parent.GetComponent<PanelManager>().UpdatePanel();
        }
    }

    public void HandleMaxLength()
    {
        int currentLength = 0;
        if (maxLength != -1)
        {
            foreach (Transform child in transform)
            {
                if (currentLength > maxLength)
                {
                    Destroy(child.gameObject);
                }
                if (child.gameObject.GetComponent<ExpressionManager>() != null)
                {
                    
                    currentLength++;
                }
                Debug.Log(currentLength);
            }
        }
    }
}
