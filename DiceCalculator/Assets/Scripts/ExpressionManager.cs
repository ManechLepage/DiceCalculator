using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ExpressionType
{
    Value,
    Operator,
    Parentesis,
    MinBracket,
    MaxBracket
}

public class ExpressionManager : MonoBehaviour
{
    public List<ExpressionManager> expressions = new List<ExpressionManager>();
    public string value;
    public ExpressionType type;
    public bool hasOperators = true;
    public int maxLength = -1;
    public Colors colors;

    public void Destroy()
    {
        Destroy(gameObject);
        gameObject.transform.parent.GetComponent<PanelManager>().UpdatePanel();
    }

    public void Update()
    {
        if (type != ExpressionType.Operator)
        {
            gameObject.GetComponent<Image>().color = colors.colors[(CalculateNestingLevel(transform) - 2) % colors.colors.Count];  
        }
    }

    int CalculateNestingLevel(Transform currentTransform, int currentLevel = 0)
    {
        if (currentTransform.parent == null)
        {
            return currentLevel;
        }
        else
        {
            return CalculateNestingLevel(currentTransform.parent, currentLevel + 1);
        }
    }


}
