using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public int chartType;
    public int chartDataType;
    public List<Vector2> plotValues = new List<Vector2>();

    public void ChangeChartType(int type)
    {
        chartType = type;
    }

    public void ChangeChartDataType(int type)
    {
        chartDataType = type;
    }


    public void CreateGraph() 
    {
        if (chartType == 0)
        {
            GetComponent<GraphManager>().GenerateBarGraph(plotValues);
        }
    }

}
