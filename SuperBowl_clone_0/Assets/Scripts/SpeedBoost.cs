using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public Collider target;
    public testController player;
    public Rigidbody rb;
    public bool triggered = false;
    public float volume = 0.5f;
    public AudioSource aS;
    public AudioClip aC;
    void OnTriggerEnter(Collider collision)
    {
        if (collision != target)
        {
            return;
        }
        else
        {
            rb.AddForce(-Vector3.right * 200);
            triggered = true;
            aS.PlayOneShot(aC, volume);
        }
    }
}
