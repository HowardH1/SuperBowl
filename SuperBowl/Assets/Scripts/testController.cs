using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testController : MonoBehaviour
{
    public Rigidbody rb;
    public FollowPlayer camera;
    public float speed;
    public float Turn_Radius;
    public float shotBounds;
    public bool canBowl = true;
    public Vector3 originalPos;
    public Quaternion originalRot;
    public float angleTransformBuffer;

    private float angleTransform;
    private float speedBuffer;
    private bool canAngleTransform = true;

    private void Start()
    {
        originalPos = gameObject.transform.position;
        originalRot = gameObject.transform.rotation;
        speedBuffer = speed;
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        angleTransform = Input.GetAxis("Angle Ball Transform");
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        checkTransformAngleBoundary();
        if (canBowl == true)
        {
            bowl();
        }

        //Vector3 dirmovement = new Vector3(-moveVertical, 0.0f, moveHorizontal);

        //rb.AddForce(dirmovement * speed);
    }

    private void checkTransformAngleBoundary()
    {
        if (angleTransformBuffer >= shotBounds || angleTransformBuffer <= -shotBounds)
        {
            canAngleTransform = false;
            if (angleTransformBuffer >= shotBounds)
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyUp(KeyCode.A) || Input.GetKey(KeyCode.A))
                {
                    angleTransformZAxis();
                }
            }
            else if (angleTransformBuffer <= -shotBounds)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyUp(KeyCode.D) || Input.GetKey(KeyCode.D))
                {
                    angleTransformZAxis();
                }
            }
        }
        else
        {
            angleTransformZAxis();
        }
    }

    private void bowl()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space) || Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Camera.main.transform.forward * speed * 17);
            if (speed > 0)
            {
                camera.canRotate = true;
                canBowl = false;
            }
        }
    }

    public void angleTransformZAxis()
    {
        canAngleTransform = true;
        float xPos = gameObject.transform.position.x;
        float yPos = gameObject.transform.position.y;
        Vector3 newPos = new Vector3(0, 0, angleTransform);
        gameObject.transform.position += (newPos / 10);
        angleTransformBuffer += angleTransform;
    }

    //Functions to remove player control during cutscenes or non-game instances
    public void removeSpeedForCinematic()
    {
        speed = 0;
    }
}