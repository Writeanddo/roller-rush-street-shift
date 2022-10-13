using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerData))]

public class PlayerInputs : MonoBehaviour
{
    [HideInInspector] public float horizontalMove;
    [HideInInspector] public bool jump = false;
    [HideInInspector] public bool crouching = false;
    [HideInInspector] public bool figure = false;
    [HideInInspector] public bool fastFalling= false;
    [HideInInspector] public bool tackle = false;

    private PlayerData playerData;

    void Start()
    {
        playerData = GetComponent<PlayerData>();
    }

    public void MoveInputs(InputAction.CallbackContext context)
    {
        horizontalMove = context.ReadValue<Vector2>().x;

        if (context.ReadValue<Vector2>().y < 0)
            fastFalling = true;
        else
            fastFalling = false;


    }
    public void TackleInput(InputAction.CallbackContext context)
    {
        
        tackle = true;

        if (context.canceled)
            tackle = false;
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
            jump = true;
    }

    public void CrouchingInput(InputAction.CallbackContext context)
    {
        crouching = true;

        if(context.canceled)
            crouching = false;
    }

    public void FigureInput(InputAction.CallbackContext context)
    {
        figure = true;

        if (context.canceled)
            figure = false;
    }

    public void BringPauseMenu(InputAction.CallbackContext context)
    {
        if(playerData.pauseMenu != null)
        {
            playerData.pauseMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(playerData.pauseMenu.GetComponentInChildren<Button>().gameObject); //Awfull
            Debug.Log(playerData.pauseMenu.GetComponentInChildren<Button>().gameObject);
            Time.timeScale = 0f;
        }
    }
}
