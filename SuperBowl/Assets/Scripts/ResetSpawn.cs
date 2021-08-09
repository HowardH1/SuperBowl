using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSpawn : MonoBehaviour
{
    public Collider target;
    public testController player;
    public Rigidbody rb;
    public bool triggered = false;
    void OnTriggerEnter(Collider collision)
    {
        if (collision != target)
        {
            return;
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            player.resetBallPosition();
        }
    }
}
