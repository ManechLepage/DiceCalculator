using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAddingScript : MonoBehaviour
{
    public GameObject searchingObject;
    public bool isSearching = false;

    public void EnableSearching()
    {
        isSearching = !isSearching;
        searchingObject.SetActive(isSearching);
    }
}
