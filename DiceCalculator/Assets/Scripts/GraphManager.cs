using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphManager : MonoBehaviour
{

    [Header("Bar Graph Settings")]
    public GameObject barPrefab;
    public Color barColor1;
    public Color barColor2;
    public float barHeightModifier = 50f;

    public void Start()
    {
        
    }

    public void GenerateBarGraph(List<Vector2> plotValues)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < plotValues.Count; i++)
        {
            GameObject bar = Instantiate(barPrefab, transform);
            bar.GetComponent<RectTransform>().sizeDelta = new Vector3(50, plotValues[i].y * barHeightModifier, 1);
            bar.GetComponent<Image>().color = LerpColor(barColor1, barColor2, plotValues[i].y);
        }
    }

    
    public Color LerpColor(Color color1, Color color2, float t)
    {
        return new Color(
            Mathf.Lerp(color1.r, color2.r, t),
            Mathf.Lerp(color1.g, color2.g, t),
            Mathf.Lerp(color1.b, color2.b, t),
            Mathf.Lerp(color1.a, color2.a, t)
        );
    }
}
