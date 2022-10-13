using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightPlayerMovement : MonoBehaviour
{

    Rigidbody rb;
    PlayerInputs inputs;
    PlayerData data;
    float currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputs = GetComponent<PlayerInputs>();
        data = GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (data.failFigure)
            return;

        Vector2 horizontalMvt = new Vector2(rb.velocity.x, rb.velocity.z);
        currentSpeed = horizontalMvt.magnitude;
        
        CheckOnGround();
        Move();
        Jump();
        Gravity();
        // StickToGround(); //TODO (trello : Physique qui colle)
        LimitSpeed();
    }

    private void LimitSpeed()
    {
        Vector2 horizontalSpeed = new Vector2(rb.velocity.x, rb.velocity.z);

        if (horizontalSpeed.magnitude > data.maxHorizontalSpeed)
        {
            horizontalSpeed = horizontalSpeed.normalized * data.maxHorizontalSpeed;
            rb.velocity = new Vector3(horizontalSpeed.x, rb.velocity.y, horizontalSpeed.y);
        }
    }

    private void Gravity()
    {
        AddGravity();
        AddFallingGravity();
        AddFastFallingGravity();
    }
    private void AddGravity()
    {
        if (!data.onGround)
            rb.velocity += Physics.gravity * data.gravity * Time.deltaTime;
    }

    private void AddFallingGravity()
    {
        if (!data.onGround && rb.velocity.y < 0)
            rb.velocity += Physics.gravity * data.fallingGravity * Time.deltaTime;
    }
    private void AddFastFallingGravity()
    {
        if (!data.onGround && inputs.fastFalling)
        {
            rb.velocity += Physics.gravity * data.fastFallingGravity * Time.deltaTime;
        }
    }


    private void Jump()
    {
        if (inputs.jump && data.onGround)
        {
            rb.AddForce(Vector2.up * data.jumpHeight, ForceMode.VelocityChange);
            inputs.jump = false;
        }
    }

    private void CheckOnGround()
    {
        if (Physics.Raycast(transform.position, -transform.up, data.groundDetection))
            data.onGround = true;
        else
            data.onGround = false;
    }

    private void Move()
    {

        float speed = data.acceleration;
        if (!data.onGround)
            speed = data.speedAirControl;


        if (inputs.horizontalMove > 0.1f)
            data.goOnRight = true;
        else if (inputs.horizontalMove < -0.1f)
            data.goOnRight = false;

        rb.velocity = new Vector3( rb.velocity.x + inputs.horizontalMove * speed * Time.deltaTime, rb.velocity.y, rb.velocity.z );
    }
}
