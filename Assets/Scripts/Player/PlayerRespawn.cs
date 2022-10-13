using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerRespawn : MonoBehaviour
{
    private PlayerData data;
    private Rigidbody rb;
    private TurnSystem turnSystem;

    private Checkpoint[] checkpoints;


    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<PlayerData>();
        rb = GetComponent<Rigidbody>();
        turnSystem = GetComponent<TurnSystem>();
        checkpoints = GameObject.Find("GameManager").GetComponent<LevelData>().checkpoints;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Respawn();
    //}

    //private void Respawn()
    //{
    //    if (transform.position.y <= data.respawnHeight) // need to respawn
    //    {
    //        rb.velocity = Vector3.zero;
    //        transform.position = checkpoints[turnSystem.nextCheckpointId - 1].transform.position;
        //}
    //}
}
