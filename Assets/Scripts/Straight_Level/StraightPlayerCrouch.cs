using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightPlayerCrouch : MonoBehaviour
{

    private PlayerInputs inputs;
    private PlayerData data;
    private Rigidbody rb;
    private Animator animator;
    private TrailRenderer trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponent<PlayerInputs>();
        data = GetComponent<PlayerData>();
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Crouching();
    }

    private void Crouching()
    {
        if (inputs.crouching)
        {
            rb.velocity += Physics.gravity * data.attractionForce * Time.deltaTime;

            animator.SetBool("Crouching", true);
            trailRenderer.enabled = true;

            RaycastHit hit;
            bool rightRaycast = Physics.Raycast(transform.position + new Vector3(.25f, 0f, 0f), new Vector3(1,0,0) - new Vector3(0, 1, 0), out hit, 1.5f);
            bool leftRaycast = Physics.Raycast(transform.position + new Vector3(.25f, 0f, 0f), new Vector3(-1, 0, 0) - new Vector3(0, 1, 0), out hit, 1.5f);

            if (rightRaycast && !leftRaycast) // Right drop
                rb.velocity = new Vector3(rb.velocity.x - data.crouchingSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);

            else if (!rightRaycast && leftRaycast) // Left drop
                rb.velocity = new Vector3(rb.velocity.x + data.crouchingSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            animator.SetBool("Crouching", false);
            trailRenderer.enabled = false;
        }
    }
}
