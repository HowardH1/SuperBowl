using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testController : MonoBehaviour
{
    public Rigidbody rb;
    public FollowPlayer camera;
    public float timeLeft = 20;
    public float speed;
    public float Turn_Radius;
    public float shotBounds;
    public GameObject[] Pins = new GameObject[10];
    public Rigidbody[] pinRB = new Rigidbody[10];
    
    private float angleTransform;
    private float angleTransformBuffer;
    private float speedBuffer;
    private float timeBuffer;
    private bool canAngleTransform = true;
    private bool resetTimer = false;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private Vector3[] pinPos = new Vector3[10];
    private Quaternion[] pinRot = new Quaternion[10];


    private void Start()
    {
        timeBuffer = timeLeft;
        for(int pin = 0; pin < Pins.Length; pin++)
        {
            pinPos[pin] = Pins[pin].transform.position;
            pinRot[pin] = Pins[pin].transform.rotation;
        }
        originalPos = gameObject.transform.position;
        originalRot = gameObject.transform.rotation;
        speedBuffer = speed;
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (resetTimer == true)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                resetBallPosition();
            }
        }
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
                resetTimer = true;
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

    public void resetBallPosition()
    {
        gameObject.transform.position = originalPos;
        gameObject.transform.rotation = originalRot;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        for (int pin = 0; pin < 10; pin++)
        {
            pinRB[pin].velocity = Vector3.zero;
            pinRB[pin].angularVelocity = Vector3.zero;
            Pins[pin].transform.position = pinPos[pin];
            Pins[pin].transform.rotation = pinRot[pin];
            Debug.Log("Pin " + pin + " reset");
        }
        resetTimer = false;
        timeLeft = timeBuffer;
    }
}