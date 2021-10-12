using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeLift : MonoBehaviour
{
    public Collider target;
    public testController player;
    public Rigidbody rb;
    public GameObject bridge;
    public GameObject bridgeTarget;
    public int speed;
    public int height;
    public int heightLimit;
    public bool triggered = false;
    public bool returnToOrigin = false;
    public Vector3 bridgeBuffer;

    private void Start()
    {
        bridgeBuffer = bridge.transform.position;
    }

    private void Update()
    {
        if(triggered == true)
        {
            StartCoroutine(WaitCoroutine());
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

    IEnumerator WaitCoroutine()
    {
        bridge.transform.position = Vector3.MoveTowards(bridge.transform.position, bridgeTarget.transform.position, speed * Time.deltaTime);

        yield return new WaitForSeconds(5f);

        bridge.transform.position = Vector3.MoveTowards(bridge.transform.position, bridgeBuffer, speed * Time.deltaTime);
        triggered = false;      
    }
}
