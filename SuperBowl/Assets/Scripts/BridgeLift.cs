using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeLift : MonoBehaviour
{
    public Collider target;
    public testController player;
    public Rigidbody rb;
    public GameObject bridge;
    public int heightLimit;
    public bool triggered = false;
    public bool returnToOrigin = false;
    public int height;
    public Vector3 bridgeBuffer;

    private void Start()
    {
        bridgeBuffer = bridge.transform.position;
    }

    private void Update()
    {
        checkBridgeMovement();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision != target)
        {
            return;
        }
        else
        {
            triggered = true;
        }
    }

    private void checkBridgeMovement()
    {
        if (triggered == true)
        {
            Debug.Log("IF 0");
            if (bridge.transform.position.y !>= heightLimit && returnToOrigin == false)
            {
                bridge.transform.Translate(Vector3.up * Time.deltaTime, Space.World);
                Debug.Log("IF 1");
            }
            else if (bridge.transform.position.y >= heightLimit)
            {
                returnToOrigin = true;
                bridge.transform.Translate(Vector3.down * Time.deltaTime, Space.World);
                Debug.Log("IF 2");
            }
            else if (bridge.transform.position.y <= bridgeBuffer.y && returnToOrigin == true)
            {
                bridge.transform.position = bridgeBuffer;
                returnToOrigin = false;
                triggered = false;
                Debug.Log("IF 3");
            }
        }
    }
}
