using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]

public class Lobby : MonoBehaviour
{
    public ColorSelection[] colorsSelectors;
    public Material[] possibleColors;

    public PlayerSpawnablesSO playerToSpawnInRace;
    public levelSelectedSO levelToLoad;
   

    [SerializeField] private Canvas confirmationCanvas;

    [HideInInspector] public PlayerInputManager manager;
    [HideInInspector] public int playersReadyCount = 0;

    private Animator transitionAnimator;

    private bool countdownStarted = false;

    private void Start()
    {
        transitionAnimator = GameObject.Find("Transition").GetComponent<Animator>();
        manager = GetComponent<PlayerInputManager>();
        playerToSpawnInRace.PlayerSpawnables = new List<PlayerSpawnable>();     //Clear the list to avoid conflics between games
    }

    void Update()
    {
        if(manager.playerCount != 0 && playersReadyCount == manager.playerCount && !countdownStarted)
        {
            StartCoroutine(ConfirmationCountdown());
        }
    }

    public void PlayerJoinLobby()
    {
        PlayerSpawnable newPlayer = new PlayerSpawnable();       
        playerToSpawnInRace.PlayerSpawnables.Add(newPlayer);
    }

    IEnumerator ConfirmationCountdown()
    {
        countdownStarted = true;
        
        float elapsedTime = 0f;

        Image[] countdown = confirmationCanvas.GetComponentsInChildren<Image>();

        Color newColor = Color.white;
        foreach (Image image in countdown)
        {
            do
            {
                newColor.a = Mathf.Lerp(255, 0, elapsedTime / 1); //elapsedTime / 1 sec
                image.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return Time.deltaTime;
            } while (image.color.a > 0 && playersReadyCount == manager.playerCount);
            elapsedTime = 0;
        }

        //if all players are still ready
        if (playersReadyCount == manager.playerCount)
        {
            transitionAnimator.SetTrigger("TransitionIn");
            transitionAnimator.SetTrigger("TransitionOut");

            float animationTime = LoadGameScene.GetTimeToWaitForAnimationToEnd("TransitionIn", transitionAnimator);
            yield return new WaitForSeconds(animationTime);

            SceneManager.LoadScene(levelToLoad.levelName, LoadSceneMode.Single);
            Debug.Log($"Loading level \"{levelToLoad.levelName}\"");
        }
        else
        {
            countdownStarted = false;
            Color transparent = Color.white;
            transparent.a = 0;
            foreach (Image image in countdown)
            {
                image.color = transparent;
            }
        }

        //TODO : Use the transition then load the scene
    }
}