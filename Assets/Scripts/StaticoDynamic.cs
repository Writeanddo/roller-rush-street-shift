using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticoDynamic : MonoBehaviour
{

    private int playerCount;
    private float randomNumber;
    private System.Random rand;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rand = new System.Random();
        randomNumber = rand.Next(0, 100);
        randomNumber /= 100;

        animator.SetFloat("RandomNumber", randomNumber);
        animator.SetInteger("PlayerCount", playerCount);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerData>())
            playerCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PlayerData>())
            playerCount--;
    }

}
