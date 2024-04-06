using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAddingScript : MonoBehaviour
{
    public GameObject searchingObject;
    public bool isSearching = false;
    
    public void Start()
    {
        searchingObject = GameObject.Find("SearchComponent");
    }

    public void EnableSearching(GameObject parent)
    {
        isSearching = !isSearching;
        searchingObject.SetActive(isSearching);
        searchingObject.GetComponent<SearchingManager>().EnableSearching(parent, gameObject);
        searchingObject.transform.position = new Vector3(gameObject.transform.position.x + 30f, gameObject.transform.position.y + 90f, gameObject.transform.position.z);
    }
}
