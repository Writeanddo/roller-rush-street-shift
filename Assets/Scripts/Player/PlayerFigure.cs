using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerFigure : MonoBehaviour
{
    private PlayerData data;
    private PlayerInputs inputs;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    [HideInInspector] public float timerFigure = 0;

    // Success
    private float figureCount = 0f;

    // Graphic
    public Canvas figureUI;

    private Animator animator;
    private Material materialColor;
    private Color playerColor;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<PlayerData>();
        inputs = GetComponent<PlayerInputs>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        materialColor = GetComponent<MeshRenderer>().material;
        playerColor = materialColor.color;
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Figure();
        DrawFigure();
    }

    void Figure()
    {
        if (inputs.figure && !inputs.tackle 
            && !Physics.Raycast(new Vector3(transform.position.x + capsuleCollider.radius, transform.position.y, transform.position.z), -transform.up, data.startFigureMinGround)
            && !Physics.Raycast(new Vector3(transform.position.x - capsuleCollider.radius, transform.position.y, transform.position.z), -transform.up, data.startFigureMinGround)
            && !data.onGround) // Figure
            data.doFigure = true;

        if(data.doFigure && data.onGround && timerFigure > 0f) // Fail
        {
            StartCoroutine(FailFigure());
        }
        else if (data.onGround && figureCount != 0f) // Success figure
        {
            StartCoroutine(SuccessFigure());
        }
        else if(!inputs.figure) // Stop
        {
            timerFigure = 0f;
            data.doFigure = false;

            // Animation
            animator.SetBool("Figure", false);
        }
        else if(data.doFigure)
        {
            timerFigure+=Time.deltaTime ;
            if (timerFigure >= data.figureTime)
            {
                if (figureCount == 0f)
                    figureCount = Mathf.Exp(1f);
                else if (figureCount == Mathf.Exp(1f))
                    figureCount = 3f;
                else
                    figureCount++;

                timerFigure = 0f;
            }

            // Animation
            animator.SetBool("Figure", true);
        }
    }

    IEnumerator SuccessFigure()
    {
        float direction = inputs.horizontalMove;
        if (direction < 0.1f && direction > -0.1f)
            direction = 1f;

//        rb.velocity += transform.forward * direction * (data.figureBoostSpeed * Mathf.Log(figureCount)) * 160 * Time.deltaTime;
        rb.velocity += transform.forward * direction * (data.figureBoostSpeed * Mathf.Log(figureCount)) * 160 * Time.deltaTime;
        figureCount = 0f;
        timerFigure = 0f;
        data.successFigure = true;
        data.doFigure = false;

        // Graphics
        animator.SetBool("SuccessFigure", true);
        materialColor.SetColor("_Color", data.colorSuccessFigure);

        yield return new WaitForSeconds(data.timeAnimationSuccessFigure);

        data.successFigure = false;
        animator.SetBool("SuccessFigure", false);
        materialColor.SetColor("_Color", playerColor);
    }

    IEnumerator FailFigure()
    {
        rb.velocity = Vector3.zero;
        figureCount = 0f;
        timerFigure = 0f;
        data.failFigure = true;
        data.doFigure = false;

        // Graphics
        animator.SetBool("FailFigure", true);
        animator.SetBool("Figure", false);
        materialColor.SetColor("_Color", data.colorFailFigure);

        yield return new WaitForSeconds(data.timeStopByFailFigure);

        data.failFigure = false;
        animator.SetBool("FailFigure", false);
        materialColor.SetColor("_Color", playerColor);
    }

    void DrawFigure()
    {
        figureUI.transform.position = transform.position;
        figureUI.transform.LookAt(Vector3.zero);
        figureUI.GetComponentInChildren<Image>().fillAmount = timerFigure / data.figureTime;
    }

    private void OnCollisionEnter(Collision collision) // Check for front collision
    {
        if(data.doFigure)
            StartCoroutine(FailFigure());
    }
}
