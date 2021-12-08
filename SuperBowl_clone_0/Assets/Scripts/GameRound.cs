using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRound : MonoBehaviour
{
    public GameObject[] players = new GameObject[4];
    public int[] points = new int[4];
    public int[,,] turnScoreHolder = new int[4, 12, 2];
    public GameObject[] Pins = new GameObject[10];
    public Rigidbody[] pinRB = new Rigidbody[10];
    public Dictionary<GameObject, int> GameData = new Dictionary<GameObject, int>();
    public testController player;

    private Vector3[] pinPos = new Vector3[10];
    private Quaternion[] pinRot = new Quaternion[10];

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        try
        {
            foreach (GameObject i in players)
            {
                foreach (int j in points)
                {
                    GameData.Add(i, j);
                }
            }
        }
        catch (ArgumentException)
        {
        }


        for (int pin = 0; pin < Pins.Length; pin++)
        {
            pinPos[pin] = Pins[pin].transform.position;
            pinRot[pin] = Pins[pin].transform.rotation;
        }
        player.originalPos = gameObject.transform.position;
        player.originalRot = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyUp(KeyCode.E) || Input.GetKey(KeyCode.E))
        {
            resetLevelPosition();
        }
    }


    public void resetLevelPosition()
    {
        player.gameObject.transform.position = player.originalPos;
        player.gameObject.transform.rotation = player.originalRot;
        player.rb.velocity = Vector3.zero;
        player.rb.angularVelocity = Vector3.zero;
        for (int pin = 0; pin < 10; pin++)
        {
            pinRB[pin].velocity = Vector3.zero;
            pinRB[pin].angularVelocity = Vector3.zero;
            Pins[pin].transform.position = pinPos[pin];
            Pins[pin].transform.rotation = pinRot[pin];
            Pins[pin].GetComponent<PinCollision>().wasHitOnce = false;
            Debug.Log("Pin " + pin + " reset");
        }
        player.angleTransformBuffer = 0;
        player.camera.canRotate = false;
        player.canBowl = true;
    }

    public void ScoreKeeper()
    {

    }
}