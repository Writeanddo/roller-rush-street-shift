using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    PlayerData playerData;
    Rigidbody rbPlayer;
    [SerializeField] private GameObject playerCenter;

    private Vector3 velocity = Vector3.zero;
    private Vector3 desiredPosition;

    float timer = 0f;
    float desiredOffset;

    private void Start()
    {
        playerData = player.GetComponent<PlayerData>();
        rbPlayer = player.GetComponent<Rigidbody>();
        desiredOffset = playerData.minCameraOffset.x + 10;
    }



    private void FixedUpdate()
    {
        Vector3 offset = playerData.currentCameraOffset;

        // Direction
        if (Vector3.Dot(player.transform.forward, rbPlayer.velocity) < 0.1f)
            offset.x -= Mathf.Lerp(offset.x, desiredOffset, timer / playerData.swapDirectionTime);
        else
            offset.x += Mathf.Lerp(offset.x, desiredOffset, timer / playerData.swapDirectionTime);

        timer += Time.deltaTime;

        Vector3 direction = (playerCenter.transform.position - transform.position).normalized;
        Quaternion rotationGoal = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationGoal, playerData.rotationSpeed);



        desiredPosition = player.transform.position + transform.right*offset.x + transform.up*offset.y + transform.forward*offset.z;

        

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, playerData.smoothSpeed);
    }
}
