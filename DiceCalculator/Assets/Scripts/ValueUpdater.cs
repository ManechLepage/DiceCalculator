using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValueUpdater : MonoBehaviour
{
    public ExpressionManager expression;
    public TMP_Dropdown dropdown;

    public void UpdateValue()
    {
        expression.value = dropdown.options[dropdown.value].text;
    }
}
