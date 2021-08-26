using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeLift : MonoBehaviour
{
    public Collider target;
    public testController player;
    public Rigidbody rb;
    public GameObject bridge;
    public bool triggered = false;
    private Vector3 bridgeBuffer;

    private void Start()
    {
        bridgeBuffer = bridge.transform.position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision != target)
        {
            return;
        }
        else
        {
            bridge.transform.position = Vector3.Lerp(bridgeBuffer, new Vector3(0, bridge.transform.position.y + 15, 0), 3f );
            triggered = true;
        }
    }
}
