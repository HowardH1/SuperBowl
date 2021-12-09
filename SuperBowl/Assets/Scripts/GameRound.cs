using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class GameRound : NetworkBehaviour
{
    public GameObject[] players = new GameObject[4];
    public int[] points = new int[4];
    public int[,,] turnScoreHolder = new int[4, 12, 2];
    public GameObject[] Pins = new GameObject[10];
    public Rigidbody[] pinRB = new Rigidbody[10];
    public Dictionary<GameObject, int> GameData = new Dictionary<GameObject, int>();
    public NetworkVariableInt activeTurnInt = new NetworkVariableInt(0);
    public NetworkVariableInt dormantTurnInt = new NetworkVariableInt(0);


    private Vector3[] pinPos = new Vector3[10];
    private Quaternion[] pinRot = new Quaternion[10];

    // Start is called before the first frame update
    void Start()
    {
        //Sets initial cameraBuffer as a reference for viewing the first player
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
    }

    [ClientRpc]
    private void cycleTurnCountClientRpc() 
    {
        int activeNetworkedTurn = gameObject.GetComponent<GameRound>().activeTurnInt.Value;
        int dormantNetworkedTurn = gameObject.GetComponent<GameRound>().dormantTurnInt.Value;

        if (activeNetworkedTurn < players.Length - 1)
        {
            dormantNetworkedTurn = activeNetworkedTurn;
            activeNetworkedTurn++;
            Debug.LogWarning("Player " + (activeNetworkedTurn + 1) + "'s turn");
        }
        else
        {
            dormantNetworkedTurn = activeNetworkedTurn;
            activeNetworkedTurn = 0;
            Debug.LogWarning("Player " + (activeNetworkedTurn + 1) + "'s turn");
        }
        gameObject.GetComponent<GameRound>().dormantTurnInt.Value = dormantNetworkedTurn;
        gameObject.GetComponent<GameRound>().activeTurnInt.Value = activeNetworkedTurn;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            changeTurnClientRpc();
        }   
    }

    [ClientRpc]
    private void changeTurnClientRpc()
    {
        if(players.Length <= 1)
        {
            players[0].GetComponent<MP_PlayerAttributes>().ResetPlayerClientRpc();
        }
        else
        {
            int activeNetworkedTurn = gameObject.GetComponent<GameRound>().activeTurnInt.Value;
            int dormantNetworkedTurn = gameObject.GetComponent<GameRound>().dormantTurnInt.Value;
            //Set new locations, active player moves to stage, dormant player moves to storage
            cycleTurnCountClientRpc();
            players[activeNetworkedTurn].GetComponent<MP_PlayerAttributes>().ResetPlayerClientRpc();
            players[dormantNetworkedTurn].GetComponent<MP_PlayerAttributes>().StorePlayerClientRpc(dormantNetworkedTurn);
            foreach (GameObject player in players)
            {
                player.GetComponentInChildren<FollowPlayer>().target = players[activeNetworkedTurn].transform;
            }
        }
        for (int pin = 0; pin < 10; pin++)
        {
            pinRB[pin].velocity = Vector3.zero;
            pinRB[pin].angularVelocity = Vector3.zero;
            Pins[pin].transform.position = pinPos[pin];
            Pins[pin].transform.rotation = pinRot[pin];
            Pins[pin].GetComponent<PinCollision>().wasHitOnce = false;
            Debug.Log("Pin " + pin + " reset");
        }
    }
}