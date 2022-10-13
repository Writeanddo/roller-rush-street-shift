using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(MultiplayerEventSystem))]

public class P_Selector : MonoBehaviour
{
    private Lobby lobbyScript;
    private GameObject lobbyGameObject;
    private MultiplayerEventSystem eventSystem;
    private ColorSelection colorSelection;

    void Start()
    {
        eventSystem = GetComponent<MultiplayerEventSystem>();

        const int arrayOffset = 1;

        lobbyGameObject = GameObject.Find("UI_Lobby");
        lobbyScript = lobbyGameObject.GetComponentInParent<Lobby>();

        eventSystem.playerRoot = lobbyGameObject;

        int playerIdx = lobbyScript.manager.playerCount - arrayOffset;

        colorSelection = lobbyScript.colorsSelectors[playerIdx];
        colorSelection.SetSelectorActive();

        GameObject button = colorSelection.buttonToAssign;
        eventSystem.SetSelectedGameObject(button);

        lobbyScript.playerToSpawnInRace.PlayerSpawnables[playerIdx].device = GetComponent<PlayerInput>().devices[0];
        colorSelection = button.GetComponentInParent<ColorSelection>();
    }

    public void CancelReadyState(InputAction.CallbackContext context)
    {
        if (colorSelection)
        {
            if (!context.canceled && colorSelection.isReady)
                colorSelection.ClickReady();
        }
    }
}