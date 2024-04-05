using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionManager : MonoBehaviour
{
    public List<ExpressionManager> expressions = new List<ExpressionManager>();
    public string value;
    
    string GetString()
    {
        return value;
    }
}
