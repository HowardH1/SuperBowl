using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRound : MonoBehaviour
{
    public GameObject[] players = new GameObject[3];
    public int[] points = new int[3];
    public Dictionary<GameObject, int> GameData = new Dictionary<GameObject, int>();

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < 4; i++)
        {
            GameData.Add(players[i], points[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPoint()
    {
        for (int i = 0; i < 4; i++)
        {

        }
            
    }
}
