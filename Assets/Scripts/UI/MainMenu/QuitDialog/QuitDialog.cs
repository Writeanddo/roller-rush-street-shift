using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuitDialog : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quiting application");
    }
}
