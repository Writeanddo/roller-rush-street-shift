using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightFollowPlayer : MonoBehaviour
{


    private PlayerData playerData;
    [SerializeField] private GameObject player;
    private Vector3 velocity = Vector3.zero;
    private Vector3 desiredPosition;
    float desiredOffset;
    float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        playerData = player.GetComponent<PlayerData>();
        desiredOffset = playerData.minCameraOffset.x + 10;
    }

    private void FixedUpdate()
    {
        Vector3 offset = playerData.minCameraOffset;

        timer += Time.deltaTime;

        transform.LookAt(player.transform.position);
        desiredPosition = player.transform.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, playerData.smoothSpeed);
    }
}
