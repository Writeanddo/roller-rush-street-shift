using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(Rigidbody))]

public class SwitchPlan : MonoBehaviour
{
    [Tooltip("Rotating part of the prefab")]
    public GameObject rotatingPart;

    private PlayerData playerData;
    private LevelData levelData;

    private HingeJoint joint;

    private bool isOnExternalTrack = true;

    private float cooldownTimer = 0f;

    private void Start()
    {
        playerData = GetComponent<PlayerData>();
        levelData = GameObject.Find("GameManager").GetComponent<LevelData>();

        joint = GetComponent<HingeJoint>();
    }

    IEnumerator ShiftPlan()
    {
        playerData.isShifting = true;

        Vector3 newConnectedAnchor = joint.connectedAnchor;

        float currentRadius, desiredRadius;
        float elapsedTime = 0f;

        const float detectionRadius = 2f;

        if (isOnExternalTrack)
        {
            currentRadius = levelData.radiusOutSideTrack;
            desiredRadius = levelData.radiusInsideTrack;
        }else
        {
            desiredRadius = levelData.radiusOutSideTrack;
            currentRadius = levelData.radiusInsideTrack;
        }

        //Smooth transition over time
        while (joint.connectedAnchor.x != desiredRadius)
        {
            if (!playerData.hitWallSwitchPlan)
            {
                Vector3 rayDirection = (rotatingPart.transform.right - transform.position).normalized * desiredRadius;

                if (!isOnExternalTrack)
                    rayDirection = -rayDirection; //Cheesy

                if (Physics.Raycast(transform.position, rayDirection, detectionRadius)) //Switch back the destination point
                {
                    float temp = desiredRadius;

                    desiredRadius = currentRadius;
                    currentRadius = temp;

                    playerData.hitWallSwitchPlan = true;
                }
            }

            newConnectedAnchor.x = Mathf.Lerp(currentRadius, desiredRadius, elapsedTime / playerData.transitionTime);
            joint.connectedAnchor = newConnectedAnchor;
            elapsedTime += Time.deltaTime;

            yield return Time.deltaTime;
        }
        
        isOnExternalTrack = !isOnExternalTrack;

        cooldownTimer = 0f;

        playerData.isShifting = false;
        playerData.hitWallSwitchPlan = false;
    }

    public void InitiateShift(InputAction.CallbackContext context)
    {
        if (playerData != null)
        {
            if (!playerData.isShifting && context.started && cooldownTimer >= playerData.shiftCooldown)
                StartCoroutine(ShiftPlan());
        }
    }

    public void Update()
    {
        cooldownTimer += Time.deltaTime;
    }
}
