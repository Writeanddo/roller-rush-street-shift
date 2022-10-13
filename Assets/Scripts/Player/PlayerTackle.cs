using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerTackle : MonoBehaviour
{

    private PlayerData data;
    private PlayerInputs inputs;
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private float tackleCooldownTimer = 0;

    public Transform meshTransform;


    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<PlayerData>();
        inputs = GetComponent<PlayerInputs>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    IEnumerator Tackle()
    {

        float tackleTimer = 0;
        animator.SetBool("Tackle", true);
        data.doingTackle = true;


        Vector2 speed = new Vector2(rb.velocity.x, rb.velocity.z);
        animator.SetBool("Tackle", true);

        while (inputs.tackle && speed.magnitude > data.tackleMinimumSpeed && tackleTimer < data.tackleMaxDuration)
        {
            if (tackleTimer >= data.tackleDurationBeforeSlow)
            {
                if (data.goOnRight)
                    rb.velocity -= transform.forward * 1 * data.tackleSlow * Time.deltaTime;
                else
                    rb.velocity -= transform.forward * -1 * data.tackleSlow * Time.deltaTime;
            }

            tackleTimer += Time.deltaTime;

            if (capsuleCollider.height == 2)
                capsuleCollider.transform.position = new Vector3(capsuleCollider.transform.position.x, capsuleCollider.transform.position.y - 1, capsuleCollider.transform.position.z);

            capsuleCollider.height = 1;

            meshTransform.rotation = Quaternion.LookRotation(transform.up, -transform.forward);

            speed.x = rb.velocity.x;
            speed.y = rb.velocity.z;

            yield return Time.deltaTime;
        }
        if (capsuleCollider.height == 1)
        {

            while (Physics.Raycast(transform.position, transform.up, 1.5f))
            {
                if (data.goOnRight)
                    transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 1, 0), -1);
                else
                    transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 1, 0), 1);
            }

            capsuleCollider.transform.position = new Vector3(capsuleCollider.transform.position.x, capsuleCollider.transform.position.y + 1, capsuleCollider.transform.position.z);
            capsuleCollider.height = 2;
            meshTransform.rotation = Quaternion.LookRotation(transform.forward, transform.up);

        }

        tackleTimer = 0;

        animator.SetBool("Tackle", false);
        data.doingTackle = false;
    }

    private void Update()
    {
        if (tackleCooldownTimer > 0 && !data.doingTackle)
            tackleCooldownTimer -= Time.deltaTime;

        if (inputs.tackle && !data.doingTackle && tackleCooldownTimer <= 0)
        {
            StartCoroutine(Tackle());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (data.doingTackle && other.GetComponent<PlayerData>() && !other.GetComponent<PlayerData>().isTackleImmune)
        {
            other.GetComponentInParent<PlayerMovement>().StunPlayer(2f, data.percentageSlowedByTackle);
        }
    }
}