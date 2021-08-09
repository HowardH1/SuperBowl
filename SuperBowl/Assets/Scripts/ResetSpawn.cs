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
            player.resetBallPosition();
        }
    }
}
