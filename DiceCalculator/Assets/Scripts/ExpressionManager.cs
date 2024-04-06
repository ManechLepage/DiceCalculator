using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExpressionType
{
    Value,
    Operator,
    Parentesis
}

public class ExpressionManager : MonoBehaviour
{
    public List<ExpressionManager> expressions = new List<ExpressionManager>();
    public string value;
    public ExpressionType type;

    public void Destroy()
    {
        Destroy(gameObject);
        gameObject.transform.parent.GetComponent<PanelManager>().UpdatePanel();
    }
}
