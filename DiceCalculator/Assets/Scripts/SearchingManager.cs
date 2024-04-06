using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchingManager : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public GameObject bracketBoxPrefab;
    
    private GameObject addBox;

    private GameObject panel;

    public void EnableSearching(GameObject parent, GameObject _addBox)
    {
        panel = parent;
        addBox = _addBox;
    }
    public void AddTextBox(GameObject text)
    {
        GameObject textBox = Instantiate(textBoxPrefab, transform);
        textBox.transform.SetParent(panel.transform);
        textBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text.GetComponent<TextMeshProUGUI>().text;
        RearangeAddBox();
        UpdatePanel();
    }

    public void AddBrackets()
    {
        GameObject bracketBox = Instantiate(bracketBoxPrefab, transform);
        bracketBox.transform.SetParent(panel.transform);
        RearangeAddBox();
        UpdatePanel();
    }

    public void RearangeAddBox()
    {
        addBox.transform.SetAsLastSibling();
        Debug.Log("Rearranged");
    }

    public void UpdatePanel()
    {
        panel.GetComponent<PanelManager>().UpdatePanel();
    }
}
