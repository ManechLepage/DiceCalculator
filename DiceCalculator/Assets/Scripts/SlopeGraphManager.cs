using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeGraphManager : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject linePrefab;
    public void GenerateSlopeGraph(List<Vector2> plotValues, bool isDragable = true)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        GameObject lastPoint = null;
        for (int i = 0; i < plotValues.Count; i++)
        {
            GameObject circle = Instantiate(circlePrefab, transform);
            circle.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, plotValues[i].y * 1000);
            circle.transform.GetChild(0).GetComponent<CircleManager>().isDragable = isDragable;

            if (i != 0)
            {
                GameObject line = Instantiate(linePrefab, transform);
                line.GetComponent<Line>().startElement = lastPoint.transform.GetChild(0).GetComponent<RectTransform>();
                line.GetComponent<Line>().endElement = circle.transform.GetChild(0).GetComponent<RectTransform>();
            }
            lastPoint = circle;
        }
    }
}
