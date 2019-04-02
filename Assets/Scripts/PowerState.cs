using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerState : MonoBehaviour
{

    public enum Powers
    {
        Blink,
        DarkVision,
        Possession,
        SlowTime
    }

    Powers currentPower;

    // Start is called before the first frame update
    void Start()
    {
        currentPower = Powers.Blink;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentPower = Powers.Blink;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentPower = Powers.DarkVision;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentPower = Powers.Possession;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentPower = Powers.SlowTime;
        }
    }
}
