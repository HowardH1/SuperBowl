using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeLift : MonoBehaviour
{
    public Collider target;
    public testController player;
    public Rigidbody rb;
    public GameObject bridge;
    public int height;
    public int speed;
    public bool triggered = false;
    private Vector3 bridgeBuffer;

    private void Start()
    {
        bridgeBuffer = bridge.transform.position;
    }

    private void Update()
    {
        if(triggered == true)
        {
            Vector3 move = new Vector3(0, 0, height);
            bridge.transform.Translate(move);
        }
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
}
