using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchingManager : MonoBehaviour
{
    public GameObject textBoxPrefab;
    
    public GameObject panel;
    public GameObject addBox;

    public void AddTextBox(GameObject text)
    {
        GameObject textBox = Instantiate(textBoxPrefab, transform);
        textBox.transform.SetParent(panel.transform);
        textBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text.GetComponent<TextMeshProUGUI>().text;
        RearangeAddBox();
    }

    public void RearangeAddBox()
    {
        addBox.transform.SetAsLastSibling();
    }
}
