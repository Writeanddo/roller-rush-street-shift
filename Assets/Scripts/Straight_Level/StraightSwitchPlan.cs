using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class StraightSwitchPlan : MonoBehaviour
{
    private PlayerData playerData;

    private bool isOnSecondPlan = true;
    private bool isShifting = false;

    private float cooldownTimer = 0f;

    [SerializeField] private float firstPlan = 1f;
    [SerializeField] private float secondPlan = 18.5f;


    private void Start()
    {
        playerData = GetComponent<PlayerData>();
    }

    IEnumerator ShiftPlan()
    {
        isShifting = true;

        bool notHitWall = true;

        float desiredPosition = firstPlan;
        float currentPosition = transform.position.z;
        float elapsedTime = 0f;

        const float detectionRadius = 1.5f;

        if (transform.position.z == firstPlan)
            desiredPosition = secondPlan;


        Vector3 rayDirection = transform.right * secondPlan;

        if (isOnSecondPlan)
            rayDirection = transform.right * -secondPlan;

        //Smooth transition over time
        while (transform.position.z != desiredPosition)
        {
            if (notHitWall)
            {
                if (Physics.Raycast(transform.position, rayDirection, detectionRadius)) //Switch back the destination point
                {
                    float temp = desiredPosition;

                    desiredPosition = currentPosition;
                    currentPosition = temp;

                    notHitWall = false;
                }
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(currentPosition, desiredPosition, elapsedTime / playerData.transitionTime));
            elapsedTime += Time.deltaTime;

            yield return Time.deltaTime;
        }

        if (notHitWall)
            isOnSecondPlan = !isOnSecondPlan;

        cooldownTimer = 0f;
        isShifting = false;
    }

    public void InitiateShift(InputAction.CallbackContext context)
    {
        if (!isShifting && context.started && cooldownTimer >= playerData.shiftCooldown)
            StartCoroutine(ShiftPlan());
    }

    public void Update()
    {
        cooldownTimer += Time.deltaTime;
    }
}
