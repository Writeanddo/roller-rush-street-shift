using System.Collections;
using UnityEngine;

public class BumperBehavior : MonoBehaviour
{
    public float jumpStrenght = 10f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<PlayerData>())
        {
            collision.transform.GetComponent<Rigidbody>().AddForce(transform.up * jumpStrenght, ForceMode.Impulse);
            StartCoroutine(AudioBumper(collision.transform.GetComponent<PlayerData>()));
        }
    }

    IEnumerator AudioBumper(PlayerData playerData)
    {
        playerData.isBumped = true;
        yield return new WaitForSeconds(2);
        playerData.isBumped = false;
    }
}
