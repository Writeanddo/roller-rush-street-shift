using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(LevelData))]
[RequireComponent(typeof(PlayerInputManager))]

public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> players = new List<GameObject>();
    [HideInInspector] public bool endRaceTimerStarted = false;

    public List<Image> positionSprites;

    [SerializeField] private PlayerSpawnablesSO playersToSpawn;
    [SerializeField] private PlayersRankingSO playersRanking;
    [SerializeField] private Canvas levelOverlay;
    [SerializeField] private GameObject pausePanel;

    private List<PlayerInput> playersInputs = new List<PlayerInput>();
    private List<PlayerData> playersData = new List<PlayerData>();
    private PlayerInputManager inputManager;
    private LevelData levelData;
    private Animator transitionAnimator;

    private float endRaceTimer;
    private bool raceIsEnding = false;

    private void Awake()
    {
        playersRanking.Clear();

        levelData = GetComponent<LevelData>();
        inputManager = GetComponent<PlayerInputManager>();

        transitionAnimator = GameObject.Find("Transition").GetComponent<Animator>();

        endRaceTimer = levelData.endRaceTimer;

        SpawnPlayers();
        StartRace();
    }

    private void UpdatePlayerData()
    {
        List<float> playersProgress = new List<float>();

        foreach (PlayerData player in playersData)
        {
            playersProgress.Add(player.progressionInTheLevel);
            if (!player.raceFinished)
                player.timer += Time.deltaTime;
        }

        for (int i = 1; i <= players.Count; i++)
        {
            float nextMaxProgress = GetMaxProgress(playersProgress);
            foreach (PlayerData player in playersData)
            {
                if (player.progressionInTheLevel == nextMaxProgress) 
                    player.rank = i;
            }
        }

    }

    private void Update()
    {
        
        //Forced to put it there as something is reseting the values of camera.rect somewhere.
        //No time to fix it proprely
        SetSplitScreenCam();

        UpdatePlayerData();

        if (RaceIsOver() && !raceIsEnding)
        {
            if (endRaceTimer <= 0)
                KickRemaingPlayers();
            StartCoroutine(LoadPodium());
        }

    }

    private float GetMaxProgress(List<float> playerProgress)
    {
        float max = 0;
        for (int i = 0; i < playerProgress.Count; i++)
        {
            if (playerProgress[i] > max)
                max = playerProgress[i];
        }
        
        playerProgress.Remove(max);
        
        return max;
    }

    private bool RaceIsOver()
    {
        if (endRaceTimerStarted)
        {
            endRaceTimer -= Time.deltaTime;

            foreach (PlayerData player in playersData)
            {
                if (!player.raceFinished && endRaceTimer > 0)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator LoadPodium()
    {
        raceIsEnding = true;

        transitionAnimator.SetTrigger("TransitionIn");
        transitionAnimator.SetTrigger("TransitionOut");

        float timeToWait = LoadGameScene.GetTimeToWaitForAnimationToEnd("TransitionIn", transitionAnimator);

        yield return new WaitForSeconds(timeToWait);

        SceneManager.LoadScene("Scene_Podium", LoadSceneMode.Single);

        raceIsEnding = false;
    }

    IEnumerator RaceCountDown()
    {
        float animationTime = LoadGameScene.GetTimeToWaitForAnimationToEnd("TransitionOut", transitionAnimator);
        yield return new WaitForSeconds(animationTime);

        float elapsedTime = 0f;

        Image[] countdown = levelOverlay.gameObject.GetComponentsInChildren<Image>(); //Assuming first component in the list is "three", second is "two", third is "one" and the fourth is "go"

        Color newColor = Color.white;
        foreach (Image image in countdown)
        {
            do
            {
                newColor.a = Mathf.Lerp(1, 0, elapsedTime / 1); //elapsedTime / 1 sec
                image.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return Time.deltaTime;
            } while (image.color.a > 0);
            elapsedTime = 0;
        }

        foreach (PlayerInput input in playersInputs)
            input.ActivateInput();
        
        StartCoroutine(TackleInvincibility());
    }

    IEnumerator TackleInvincibility()
    {
        yield return new WaitForSeconds(levelData.tackleInvincibility);

        foreach (PlayerData player in playersData)
            player.isTackleImmune = false;
    }

    private void SpawnPlayers()
    {
        foreach (PlayerSpawnable player in playersToSpawn.PlayerSpawnables)
        {
            GameObject playerSpawned = inputManager.JoinPlayer(-1, -1, null, player.device).gameObject;
            PlayerData playerData = playerSpawned.GetComponent<PlayerData>();
            PlayerInput inputs = playerSpawned.GetComponent<PlayerInput>();
            MeshRenderer mesh = playerSpawned.transform.Find("Mesh").GetComponent<MeshRenderer>();

            playerData.pauseMenu = pausePanel;
            playerData.isTackleImmune = true;
            inputs.DeactivateInput();

            mesh.material = player.playerColor;

            playersInputs.Add(inputs);
            playersData.Add(playerData);
            
            players.Add(playerSpawned);
        }

        foreach(GameObject player in players)
        {
            player.GetComponent<PlayerData>().totalPlayerCount = players.Count;
        }
    }

    private void SetSplitScreenCam()
    {
        if (players.Count == 2)
        {
            Camera camPlayer1 = players[0].GetComponent<PlayerData>().camera;
            Camera camPlayer2 = players[1].GetComponent<PlayerData>().camera;

            camPlayer1.rect = new Rect(new Vector2(0,.5f),new Vector2(1,1));
            camPlayer2.rect = new Rect(new Vector2(0, -.5f), new Vector2(1, 1));

            //problem with horizontal split screen, forces this workaround
            players[0].GetComponent<PlayerData>().minCameraOffset = new Vector3(0, 4, -18);
            players[1].GetComponent<PlayerData>().minCameraOffset = new  Vector3(0, 4, -18);
        }
    }

    private void StartRace()
    {
        StartCoroutine(RaceCountDown());
    }

    private void KickRemaingPlayers()
    {
        foreach(PlayerData player in playersData)
        {
           if(!playersRanking.PlayerDatas.Contains(player))
                player.GetComponent<TurnSystem>().FinishRace();
        }
    }
}
