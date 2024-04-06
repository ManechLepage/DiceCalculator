using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public bool isDiceCalculatorTab = true;
    public GameObject diceCalculatorTab;
    public GameObject diceCalculatorButton;
    public GameObject graphTab;
    public GameObject graphButton;

    public void DiceCalculatorButton()
    {
        isDiceCalculatorTab = true;
        diceCalculatorTab.SetActive(true);
        graphTab.SetActive(false);
        diceCalculatorButton.GetComponent<Button>().interactable = false;
        graphButton.GetComponent<Button>().interactable = true;
    }

    public void GraphButton()
    {
        isDiceCalculatorTab = false;
        diceCalculatorTab.SetActive(false);
        graphTab.SetActive(true);
        diceCalculatorButton.GetComponent<Button>().interactable = true;
        graphButton.GetComponent<Button>().interactable = false;
    }

}
