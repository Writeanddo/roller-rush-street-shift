using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToGame : MonoBehaviour
{
    public GameObject pausePanel;

    /// <summary>
    /// Hide the pause UI and un-stop the time
    /// </summary>
    public void ReturnToGameScene()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
