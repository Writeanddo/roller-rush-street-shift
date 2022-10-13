using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(TrailRenderer))]

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputs inputs;
    private PlayerData data;
    private LevelData levelData;
    private Rigidbody rb;
    private Animator animator;
    private float lastYVelocity; // use in : StickToGround()
    private Vector2 horizontalSpeed;

    public HingeJoint joint;
    public GameObject rotatingPart;

    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponent<PlayerInputs>();
        levelData = GameObject.Find("GameManager").GetComponent<LevelData>();
        data = GetComponent<PlayerData>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        joint = GetComponent<HingeJoint>();
        SpawnPlayer();
    }

    void FixedUpdate()
    {        
        if (data.failFigure)
            return;

        horizontalSpeed = new Vector2(rb.velocity.x, rb.velocity.z);

        AudioSet(); // need to be before checkOnGround()
        CheckOnGround();
        Move();
        AnimatorSet();
        Jump();
        Gravity();
        LimitSpeed();
        SetFOVChanges();
    }


    private void SetFOVChanges()
    {
        // camera offset
        data.currentCameraOffset = Vector3.Lerp(data.minCameraOffset, data.maxCameraOffset, horizontalSpeed.magnitude / data.maxHorizontalSpeed);
    }

    private void LimitSpeed()
    {
        if (horizontalSpeed.magnitude > data.maxHorizontalSpeed)
        {
            horizontalSpeed = horizontalSpeed.normalized * data.maxHorizontalSpeed;
            rb.velocity = new Vector3(horizontalSpeed.x, rb.velocity.y, horizontalSpeed.y);
        }
    }


    private void AnimatorSet()
    {
        animator.SetFloat("HorizontalMove", Vector3.Dot(transform.forward, rb.velocity));
        animator.SetFloat("VerticalMove", rb.velocity.y);

        if (!data.onGround)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("InTheAir", true);
        }
        else
            animator.SetBool("InTheAir", false);
    }

    private void AudioSet()
    {
        data.speed = rb.velocity.magnitude;
        data.lastOnGround = data.onGround;

        if(!data.onGround)
            data.jump = false;
    }

    private void Gravity()
    {
        AddGravity();
        AddFallingGravity();
        AddFastFallingGravity();
    }

    private void CheckOnGround()
    {
        if (Physics.Raycast(transform.position, -transform.up, data.groundDetection))
            data.onGround = true;
        else
            data.onGround = false;
    }

    private void Jump()
    {
        if (inputs.jump && data.onGround)
        {
            rb.AddForce(Vector2.up * data.jumpHeight, ForceMode.VelocityChange);
            inputs.jump = false;

            // Aniamtor
            animator.SetBool("Jump", true);
            // Audio
            data.jump = true;
        }
    }

    private void Move()
    {
        float speed = data.acceleration;
        if (!data.onGround)
            speed = data.speedAirControl;

        if (Vector3.Dot(transform.forward, rb.velocity) < 0f)
            data.goOnRight = false;
        else
            data.goOnRight = true;

        if(!data.doingTackle && horizontalSpeed.magnitude < data.maxControlHorizontalSpeed)
            rb.velocity += transform.forward * inputs.horizontalMove * speed * Time.deltaTime;
    }

    public float CalculateAngle()
    {
        float radian = Mathf.Atan2(transform.position.z, transform.position.x);
        float angle = radian * (180 / Mathf.PI);
        if (angle < 0.0)
            angle += 360.0f;
        return angle;
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

    private void SpawnPlayer()
    {
        transform.position = levelData.spawnPosition;
        joint.connectedAnchor = new Vector3(levelData.spawnRadius, joint.connectedAnchor.y, joint.connectedAnchor.z);
        rotatingPart.transform.position = new Vector3(rotatingPart.transform.position.x, levelData.spawnPosition.y, rotatingPart.transform.position.z);
        //TODO Optional : Set camera at right coordinates ?
    }


    public void StunPlayer(float duration, float percentage = 0f)
    {
        StartCoroutine(Stun(duration, percentage));
    }

    IEnumerator Stun(float duration, float percentage = 0f)
    {
        float stunTimer = 0;
        data.isTackleVibration = true;
        Vector3 velocityStun = rb.velocity * (percentage / 100);
        while (stunTimer < duration)
        {
            stunTimer += Time.deltaTime;
            rb.velocity = velocityStun;
            yield return Time.deltaTime;
        }
        StartCoroutine(TackleInvincibility());
    }

    IEnumerator TackleInvincibility()
    {
        PlayerTackle tackleComponent = GetComponent<PlayerTackle>();
        float timer = 0f;
        while(timer <= data.tackleInvicibilityDuration)
        {
            data.isTackleImmune = true;
            timer += Time.deltaTime;
            yield return Time.deltaTime;
        }

        data.isTackleImmune = false;
    }

}
