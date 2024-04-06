using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public int chartType;
    public int chartDataType;
    public GameObject barGraph;
    public GameObject slopeGraph;
    public List<Vector2> plotValues = new List<Vector2>();
    public List<Vector2> tmpPlotValues = new List<Vector2>();
    public void Awake()
    {
        tmpPlotValues = plotValues;
    }

    public void ChangeChartType(int type)
    {
        chartType = type;
        plotValues = tmpPlotValues;
        if (chartType == 0)
        {
            barGraph.SetActive(true);
            slopeGraph.SetActive(false);
        }
        else if (chartType == 1)
        {
            barGraph.SetActive(false);
            slopeGraph.SetActive(true);
        }
        CreateGraph();
    }

    public void ChangeChartDataType(int type)
    {
        chartDataType = type;
        plotValues = tmpPlotValues;
        if (chartDataType == 0)
        {
            plotValues = tmpPlotValues;
        }
        else if (chartDataType == 1)
        {
            TransformPlotValuesToAtLeast();
        }
        else if (chartDataType == 2)
        {
            TransformPlotValuesToAtMost();
        }
        CreateGraph();
    }

    public void TransformPlotValuesToAtLeast()
    {
        float sum = 1;
        for (int i = 0; i < plotValues.Count; i++)
        {
            float tmpSum = sum;
            sum -= plotValues[i].y;
            plotValues[i] = new Vector2(tmpPlotValues[i].x, tmpSum);
        }
    }

    public void TransformPlotValuesToAtMost()
    {
        float sum = 1;
        for (int i = plotValues.Count; i < 0; i--)
        {
            plotValues[i] = new Vector2(plotValues[i].x, sum);
            sum += tmpPlotValues[i].y;
        }
    }


    public void CreateGraph() 
    {
        if (chartType == 0)
        {
            barGraph.GetComponent<GraphManager>().GenerateBarGraph(plotValues);
        }
        else if (chartType == 1)
        {
            slopeGraph.GetComponent<SlopeGraphManager>().GenerateSlopeGraph(plotValues);
        }
    }

}
