using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerCrouching : MonoBehaviour
{
    private PlayerInputs inputs;
    private PlayerData data;
    private Rigidbody rb;
    private Animator animator;
    private Vector2 horizontalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponent<PlayerInputs>();
        data = GetComponent<PlayerData>();
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalSpeed = new Vector2(rb.velocity.x, rb.velocity.z);
        Crouching();
    }

    private void Crouching()
    {
        if (inputs.crouching)
        {
            if (horizontalSpeed.magnitude < data.maxCrouchingSpeed)
                rb.velocity += Physics.gravity * data.attractionForce * Time.deltaTime;

            animator.SetBool("Crouching", true);
            data.crunching = true;

            RaycastHit hit;
            bool rightRaycast = Physics.Raycast(transform.position + new Vector3(.25f, 0f, 0f), transform.forward - transform.up, out hit, 1.5f);
            bool leftRaycast = Physics.Raycast(transform.position + new Vector3(.25f, 0f, 0f), -transform.forward - transform.up, out hit, 1.5f);

            if(horizontalSpeed.magnitude < data.maxCrouchingSpeed)
            {

                if (rightRaycast && !leftRaycast) // Right drop
                    rb.velocity -= transform.forward * data.crouchingSpeed * Time.deltaTime;

                else if (!rightRaycast && leftRaycast) // Left drop
                    rb.velocity += transform.forward * data.crouchingSpeed * Time.deltaTime;
            }
        }
        else
        {
            animator.SetBool("Crouching", false);
            data.crunching = false;
        }
    }
}
