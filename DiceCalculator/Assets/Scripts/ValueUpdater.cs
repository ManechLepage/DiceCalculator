using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValueUpdater : MonoBehaviour
{
    public ExpressionManager expression;
    public TMP_Dropdown dropdown;

    void Start()
    {
        UpdateValue();
    }

    public void UpdateValue()
    {
        switch (dropdown.value)
        {
            case 0:
                expression.value = "+";
                break;
            case 1:
                expression.value = "-";
                break;
            case 2:
                expression.value = "*";
                break;
            case 3:
                expression.value = "/";
                break;
        }
    }
}
