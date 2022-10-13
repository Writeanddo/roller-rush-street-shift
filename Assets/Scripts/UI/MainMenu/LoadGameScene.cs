using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class LoadGameScene : MonoBehaviour
{
    [SerializeField] private string SceneToLoad = "";
    [SerializeField] private Animator transitionAnimator;

    public void LoadScene()
    {
        transitionAnimator = GameObject.Find("Transition").GetComponent<Animator>();
        StartCoroutine(sceneLoading());
    }

    IEnumerator sceneLoading()
    {
        EventSystem.current.SetSelectedGameObject(null);

        DontDestroyOnLoad(transitionAnimator.gameObject); //To keep the animation playing

        transitionAnimator.SetTrigger("TransitionIn");
        transitionAnimator.SetTrigger("TransitionOut");

        float timeToWait = GetTimeToWaitForAnimationToEnd("TransitionIn", transitionAnimator);

        yield return new WaitForSeconds(timeToWait);

        SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
    }


    public static float GetTimeToWaitForAnimationToEnd(string animationName , Animator animator)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        
        float timeToWait= 0;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                timeToWait = clip.length;
            }
        }

        return timeToWait;
    }
}