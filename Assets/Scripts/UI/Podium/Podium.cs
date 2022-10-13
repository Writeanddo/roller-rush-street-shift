using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Podium : MonoBehaviour
{
    [SerializeField] private float screenDuration = 500;
    [SerializeField] private string sceneToLoad = "Scene_MainMenu";

    [SerializeField] private PlayersRankingSO playerRanking;
    [SerializeField] private MeshRenderer[] meshes;
    [SerializeField] private Text[] timerTexts;

    private Animator transitionAnimator;

    // Start is called before the first frame update
    void Start()
    {
        transitionAnimator = GameObject.Find("Transition").GetComponent<Animator>();

        SetupPodium();

        StartCoroutine(victoryScreenTimer());
    }

    IEnumerator victoryScreenTimer()
    {
        float timer = 0f;

        while (timer < screenDuration)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transitionAnimator.SetTrigger("TransitionIn");
        float timeToWait = LoadGameScene.GetTimeToWaitForAnimationToEnd("TransitionIn", transitionAnimator);

        yield return new WaitForSeconds(timeToWait);

        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);

        Destroy(transitionAnimator.gameObject);
    }

    private void SetupPodium()
    {
        for(int i = 0; i < playerRanking.PlayerDatas.Count;i++)
        {
            //Get the corresponding references in the public lists
            int postitionInLists = playerRanking.PlayerDatas[i].rank - 1;
            
            MeshRenderer mesh = meshes[postitionInLists];
            Text timerText = timerTexts[postitionInLists];


            timerText.text = System.TimeSpan.FromSeconds(playerRanking.PlayerDatas[i].timer).ToString();

            mesh.material = playerRanking.PlayerMaterials[i];
            mesh.enabled = true;
        }
    }
}