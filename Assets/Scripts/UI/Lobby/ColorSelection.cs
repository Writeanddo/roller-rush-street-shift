using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO : Add feedback when the player is ready
//TODO : Add mesh for color preview

public class ColorSelection : MonoBehaviour
{
    [SerializeField] private Lobby lobby;

    [HideInInspector] public GameObject buttonToAssign;
    [HideInInspector] public bool slectorIsActive = false;

    private Navigation defaultReadyNav;
    private Navigation nullNav;

    private CanvasGroup selectionGroup;

    private Text joiningText;

    private Button leftArrow;
    private Button rightArrow; //A bit useless but it's here
    private Button readyButton;

    private MeshRenderer previewMesh;

    private int choosenColorIdx = 0;

    private bool _isReady = false;

    public bool isReady
    {
        get { return _isReady; }
        set { }
    }

    public void Awake()
    {
        GameObject panel = transform.Find("SelectorPanel").gameObject;
        
        previewMesh = GetComponentInChildren<MeshRenderer>();
        previewMesh.material = lobby.possibleColors[0];

        selectionGroup = panel.GetComponent<CanvasGroup>();

        joiningText = transform.Find("WaitingForPlayerText").GetComponent<Text>();
        
        selectionGroup.alpha = 0;

        Button[] buttons = GetComponentsInChildren<Button>();

        leftArrow = buttons[0];
        rightArrow = buttons[1];
        readyButton = buttons[2];

        buttonToAssign = leftArrow.gameObject;
        defaultReadyNav = readyButton.navigation;
        nullNav = new Navigation();
    }

    public void ClickReady()
    {
        if(!isReady)
            SetReadyState();
        else
            RemoveReadyState();
    }

    public void PreviousColor()
    {
        if (choosenColorIdx <= 0)
        {
            choosenColorIdx = lobby.possibleColors.Length - 1;
        } else
        {
            choosenColorIdx--;
        }

        previewMesh.material = lobby.possibleColors[choosenColorIdx];
    }

    public void NextColor()
    {
        choosenColorIdx++;

        if (choosenColorIdx >= lobby.possibleColors.Length)
        {
            choosenColorIdx = 0;
        }
        previewMesh.material = lobby.possibleColors[choosenColorIdx];
    }

    private void SetReadyState()
    { 
        _isReady = true;

        readyButton.navigation = nullNav; //Lock the player on the ready button in case he want to cancel
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);

        lobby.playersReadyCount++;

        SendColorDataTolobby();
    }

    private void RemoveReadyState()
    {
        _isReady = false;

        readyButton.navigation = defaultReadyNav;
        leftArrow.gameObject.SetActive(true);
        rightArrow.gameObject.SetActive(true);

        lobby.playersReadyCount--;
    }

    public void SendColorDataTolobby()
    {
        //Get the corresponding playerspawable object in put the color in it        
        for (int i = 0; i < lobby.colorsSelectors.Length; i++)
        {
            if (this == lobby.colorsSelectors[i])
            {
                lobby.playerToSpawnInRace.PlayerSpawnables[i].playerColor = lobby.possibleColors[choosenColorIdx];
                break;
            }
        }
    }

    public void SetSelectorActive()
    {
        Color tranparent = Color.white;
        tranparent.a = 0;
        selectionGroup.alpha = 1;
        slectorIsActive = true;
        previewMesh.enabled = true;

        joiningText.color = tranparent;
    }

    public void SetSelectorInactive()
    {
        joiningText.color = Color.white;
        slectorIsActive = false;
        previewMesh.enabled = false;
        selectionGroup.alpha = 0;
    }
}
