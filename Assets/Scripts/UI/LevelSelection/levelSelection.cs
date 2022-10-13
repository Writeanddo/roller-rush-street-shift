using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class levelSelection : MonoBehaviour
{

    [SerializeField] private levelSelectedSO levelSelected;
    [SerializeField] private string lobbySceneName = "Scene_Lobby";

    private Animator transitionAnimator;
    public void Start()
    {
        transitionAnimator = GameObject.Find("Transition").GetComponent<Animator>();
    }

    public void SelectLevel()
    {
        levelSelected.levelName = EventSystem.current.currentSelectedGameObject.name;
        EventSystem.current.SetSelectedGameObject(null);

        transitionAnimator.SetTrigger("TransitionIn");
        transitionAnimator.SetTrigger("TransitionOut");

        StartCoroutine(LoadLobby());
    }

    IEnumerator LoadLobby()
    {
        float animationTime = LoadGameScene.GetTimeToWaitForAnimationToEnd("TransitionIn", transitionAnimator);
        yield return new WaitForSeconds(animationTime);

        SceneManager.LoadScene(lobbySceneName, LoadSceneMode.Single);
    }
}