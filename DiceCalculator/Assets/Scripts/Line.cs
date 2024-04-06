using UnityEngine;
using UnityEngine.UI;

public class Line : MonoBehaviour
{
    public RectTransform startElement; // The RectTransform of the start UI element
    public RectTransform endElement;   // The RectTransform of the end UI element
    public float lineWidth = 2f;       // Width of the line
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        UpdateLinePosition();
    }

    void Update()
    {
        UpdateLinePosition();
    }

    void UpdateLinePosition()
    {
        if (startElement == null || endElement == null)
            return;

        Vector3 startPosition = startElement.position;
        Vector3 endPosition = endElement.position;

        Vector3 direction = endPosition - startPosition;
        float distance = direction.magnitude;
        Vector3 normalizedDirection = direction / distance;

        rectTransform.position = startPosition + (direction * 0.5f);
        rectTransform.sizeDelta = new Vector2(distance, lineWidth); // Set the width here

        float angle = Mathf.Atan2(normalizedDirection.y, normalizedDirection.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}