using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverlay : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    [SerializeField] private Text lapCountText;
    [SerializeField] private Text timerText;

    [SerializeField] private Image rankingImage;

    [SerializeField] private List<Sprite> rankSprites = new List<Sprite>(4);

    private LevelData levelData;
    private TurnSystem turnSystem;
    private Checkpoint[] checkpoints;

    void Start()
    {
        turnSystem = playerData.GetComponent<TurnSystem>();

        GameObject gameManager = GameObject.Find("GameManager");

        levelData = gameManager.GetComponent<LevelData>();
        checkpoints = gameManager.GetComponent<LevelData>().checkpoints;
    }

    public void DrawRank()
    {
        rankingImage.sprite = rankSprites[playerData.rank - 1];
    }

    public void DrawTimer()
    {
        timerText.text = System.TimeSpan.FromSeconds(playerData.timer).ToString();
    }


    private void DrawTurnCount()
    {
        lapCountText.text = (turnSystem.currentTurn + 1).ToString() + "/" + levelData.numberOfTurns.ToString();
    }

    private void UpdateLevelProgress()
    {
        // calculate the progression in the level, the value with which ranking players is possible
        playerData.progressionInTheLevel = (turnSystem.currentTurn + 1) * 1000;
        
        if(turnSystem.nextCheckpointId ==  0)
            playerData.progressionInTheLevel += checkpoints.Length * 100;
        else
            playerData.progressionInTheLevel += turnSystem.nextCheckpointId * 100;

        Vector3 nextCheckpointPosition = checkpoints[turnSystem.nextCheckpointId].transform.position;
        Vector3 vectorToNextCheckpoint = nextCheckpointPosition - transform.position;
        playerData.progressionInTheLevel -= Mathf.Abs(vectorToNextCheckpoint.magnitude);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLevelProgress();
        DrawRank();
        DrawTimer();
        DrawTurnCount();
    }
}
