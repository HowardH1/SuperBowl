using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinCollision : MonoBehaviour
{
    public bool isHit = false;
    public bool wasHitOnce = false;
    public GameRound gameController;
    private void OnCollisionEnter(Collision collision)
    {
        foreach (GameObject player in gameController.players)
        {
            foreach (int point in gameController.points)
            {
                if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Pin")
                {
                    if (getPinState() && isHit && wasHitOnce == false)
                    {
                        gameController.GameData[player] += 1;
                        wasHitOnce = true;
                        Debug.Log("Current Points: " + gameController.GameData[player]);
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }

    private bool getPinState()
    {
        if (gameObject.transform.rotation.x != 0 || gameObject.transform.rotation.z != 0)
        {
            isHit = true;
            return true;
        }
        else
        {
            Debug.Log("Pin X Coordinate: " + gameObject.transform.rotation.x + " Pin Z Coordinate" + gameObject.transform.rotation.z);
            return false;
        }
    }
}