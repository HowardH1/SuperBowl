using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResetSpawn : MonoBehaviour
{
    public Collider target;
    public GameRound gameControl;
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
            gameControl.resetLevelPosition();
        }
    }
}
