using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuTransition : MonoBehaviour
{ 
    public GameObject CurrentPanel, DestinationPanel;
    public bool HidePanelOnTransition = true;
    public GameObject NextButtonSelected;
    public void Transistion()
    {
        if (HidePanelOnTransition)
        {
            CurrentPanel.SetActive(false);
        }

        DestinationPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(NextButtonSelected);
    }
}
