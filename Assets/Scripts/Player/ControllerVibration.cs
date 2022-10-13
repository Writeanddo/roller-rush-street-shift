using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerData))]
public class ControllerVibration : MonoBehaviour
{
    PlayerData data;
    Gamepad controller = null;
    private bool alreadyVibrate = false;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<PlayerData>();

        if (GetComponent<PlayerInput>().devices[0] is Gamepad)
            controller = (Gamepad)GetComponent<PlayerInput>().devices[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (controller != null && !alreadyVibrate && (data.failFigure || data.hitWallSwitchPlan || data.isTackleVibration))
        {
            data.isTackleVibration = false;
            StartCoroutine(Vibration());
        }
    }

    private IEnumerator Vibration()
    {
        alreadyVibrate = true;
        controller.SetMotorSpeeds(data.vibrationLowFrequence, data.vibrationHighFrequence);
        yield return new WaitForSeconds(data.vibrationDuration);
        controller.SetMotorSpeeds(0f, 0f);
        alreadyVibrate = false;
    }
}
