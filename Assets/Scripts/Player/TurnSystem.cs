using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerData))]
public class TurnSystem : MonoBehaviour
{
    [HideInInspector] public int nextCheckpointId = 1;
    [HideInInspector] public int currentTurn = 0;

    [SerializeField] private PlayersRankingSO playersRanking;
    [SerializeField] private string victoryScene = "Scene_Victory";

    private Checkpoint[] checkpoints;
    private GameManager gameManager;
    private LevelData levelData;
    private PlayerData playerData;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        levelData = gameManager.GetComponent<LevelData>();
        
        checkpoints = levelData.checkpoints;

        playerData = GetComponent<PlayerData>();
    }

    public void FinishRace()
    {
        playerData.raceFinished = true;
        gameManager.endRaceTimerStarted = true;
        GetComponent<PlayerInput>().enabled = false;

        Material mat = transform.Find("Mesh").GetComponent<MeshRenderer>().material;

        playersRanking.Add(playerData, mat); //Save the rank in the Scriptable Object
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Checkpoint>() == checkpoints[nextCheckpointId])
        {        
            if (nextCheckpointId == 0) //Check if start
                currentTurn++;

            nextCheckpointId++;

            if (nextCheckpointId >= checkpoints.Length) //Check if end of list
                nextCheckpointId = 0;

            if (currentTurn >= levelData.numberOfTurns && !playerData.raceFinished) //If end of the race
                FinishRace();
        }
    }
}
