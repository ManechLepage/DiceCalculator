using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GraphGeneratorManager : MonoBehaviour
{
    public TMP_InputField value1Label;
    public TMP_InputField value2Label;
    public GameObject GraphPanel;

    public int graphResolution = 10;

    public void GenerateGraphData()
    {
        int min = Convert.ToInt32(value1Label.text);
        int max = Convert.ToInt32(value2Label.text);

        List<Vector2> plotValues = new List<Vector2>();
        for (int i = 0; i < graphResolution; i++)
        {
            float x = i / (float)graphResolution;
            float y = 0.5f;
            plotValues.Add(new Vector2(x, y));
        }

        GraphPanel.GetComponent<SlopeGraphManager>().GenerateSlopeGraph(plotValues, true);
    }
}
