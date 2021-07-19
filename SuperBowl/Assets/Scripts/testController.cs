using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    private float speedBuffer;
    public float Turn_Radius;

    private void Start()
    {
        speedBuffer = speed;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 dirmovement = new Vector3(-moveVertical, 0.0f, moveHorizontal);

        rb.AddForce(dirmovement * speed);
    }

    public void removeSpeedForCinematic()
    {
        speed = 0;
    }

    public void setSpeedPostCinematic()
    {
        speed = speedBuffer;
    }

    public void extScriptSetSpeed(int newSpeed)
    {
        speed = newSpeed;
    }
}