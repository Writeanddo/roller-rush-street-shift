using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBehavior : MonoBehaviour
{
    [SerializeField] private float boostPower = 50f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerData>())
        {
            other.GetComponent<Rigidbody>().AddForce((other.transform.forward + transform.forward) * boostPower, ForceMode.Impulse);
            other.GetComponent<PlayerData>().noSpeedLimit = true;

            /*
            Rigidbody rb = other.transform.GetComponent<Rigidbody>();
            rb.velocity += transform.forward * inputs.horizontalMove * speed * Time.deltaTime;

            rb.velocity = transform.forward

            rb.velocity = new Vector3(rb.velocity.x, boostPower, rb.velocity.z);
            */

            StartCoroutine(AudioBoost(other.transform.GetComponent<PlayerData>()));
        }
    }

    IEnumerator AudioBoost(PlayerData playerData)
    {
        playerData.isBoosted = true;
        yield return new WaitForSeconds(2);
        playerData.isBoosted = false;
    }
}
