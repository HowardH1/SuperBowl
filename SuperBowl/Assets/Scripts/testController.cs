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
    
    private float angleTransform;
    private float angleTransformBuffer;
    private float speedBuffer;
    private bool canAngleTransform = true;


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
        angleTransform = Input.GetAxis("Angle Ball Transform");
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        checkTransformAngleBoundary();
        bowl();

        //Vector3 dirmovement = new Vector3(-moveVertical, 0.0f, moveHorizontal);

        //rb.AddForce(dirmovement * speed);
    }

    private void checkTransformAngleBoundary()
    {
        if(angleTransformBuffer >= shotBounds || angleTransformBuffer <= -shotBounds)
        {
            canAngleTransform = false;
            if (angleTransformBuffer >= shotBounds)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow))
                {
                    angleTransformZAxis();
                }
            }
            else if (angleTransformBuffer <= -shotBounds)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow))
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
            rb.AddForce(Camera.main.transform.forward * speed * 5);
            if(speed > 0)
            {
                camera.canRotate = true;
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