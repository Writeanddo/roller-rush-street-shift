using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Header("Tracks")]
    [Tooltip("Distance between the center of the level and the center of the inner track")]
    public float radiusInsideTrack = 15;
    [Tooltip("Distance between the center of the level and the center of the outer track")]
    public float radiusOutSideTrack = 20;

    [Header("Spawn Point")]
    [Tooltip("The spawn point coordinate on the track")]
    [SerializeField] private float spawnAngle  = 10f;
    [SerializeField] private float spawnHeight = 0f;
    [Tooltip("The spawn point is on the outside track or not")]
    public bool spawnOnOutsideTrack = true;

    [Header("Level Info")]
    [Tooltip("The number of laps needed to finish the race")]
    public int numberOfTurns = 3;
    [Tooltip("Populate this list with all the checkpoints of the race (where start is checkpoint[0])")]
    public Checkpoint[] checkpoints;
    [Tooltip("Duration of the invincibility when the race start")]
    public float tackleInvincibility = 3f;
    [Tooltip("When a player finish the race, this is the time to wait before forcing the end of the race")]
    public float endRaceTimer = 60;

    [HideInInspector] public float spawnRadius;
    [HideInInspector] public Vector3 spawnPosition;

    public void Start()
    {
        spawnPosition = ComputeSpawnPosition();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radiusInsideTrack);
        Gizmos.DrawWireSphere(transform.position, radiusOutSideTrack);
        Gizmos.DrawCube(ComputeSpawnPosition(), Vector3.one);
    }

    private Vector3 ComputeSpawnPosition()
    {
        if (spawnOnOutsideTrack)
        {
            spawnRadius = radiusOutSideTrack;
        }
        else
        {
            spawnRadius = radiusInsideTrack;
        }

        float xCoordinate, zCoordinate;

        xCoordinate = spawnRadius * Mathf.Cos(spawnAngle);
        zCoordinate = spawnRadius * Mathf.Sin(spawnAngle);

        return transform.position + new Vector3(xCoordinate, spawnHeight, zCoordinate);
    }
}
