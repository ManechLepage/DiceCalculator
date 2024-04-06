using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlopeGraphLabel : MonoBehaviour
{
    public GameObject labelPrefab;

    public void UpdateLabels(List<Vector2> plotValues)
    {
        foreach (Transform child in transform)
        {
            Destroy(child);
        }
        for (int i = 0; i < plotValues.Count; i++)
        {
            GameObject text = Instantiate(labelPrefab, transform);
            text.GetComponent<TextMeshProUGUI>().text =  i.ToString();
        }
    }
}
