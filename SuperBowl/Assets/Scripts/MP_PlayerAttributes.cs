using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine.UI;
using MLAPI.Messaging;
using System;

public class MP_PlayerAttributes : NetworkBehaviour
{
    private float maxHp = 100f;
    public NetworkVariableBool powerUp = new NetworkVariableBool(false);
    private NetworkVariableFloat currentHp = new NetworkVariableFloat(100f);

    public NetworkVariableInt points = new NetworkVariableInt(0);
    public NetworkVariableInt deaths = new NetworkVariableInt(0);
    public NetworkVariableBool activePlayer = new NetworkVariableBool(false);


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pin") && IsOwner)
        {
            increasePointCountServerRpc();
        }
    }

    [ServerRpc]
    private void TakeDamageServerRpc(float damage, ServerRpcParams svrParams = default)
    {
        currentHp.Value -= damage;
        if(currentHp.Value <= 0 && OwnerClientId == svrParams.Receive.SenderClientId)
        {
            deaths.Value++;
        }
    }

    [ServerRpc]
    private void HealDamageServerRpc(float heal)
    {
        currentHp.Value += heal;
        if(currentHp.Value > 100)
        {
            currentHp.Value = maxHp;
        }
    }

    [ServerRpc]
    private void RespawnPlayerServerRpc()
    {
        //Set health to 100%
        currentHp.Value = maxHp;
    }

    [ClientRpc]
    public void ResetPlayerClientRpc()
    {
        //Set position to spawn point
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        GetComponent<testController>().enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.position = spawnPoint.transform.position;
        GetComponent<testController>().enabled = true;
        GetComponent<testController>().canBowl = true;
    }

    [ClientRpc]
    public void StorePlayerClientRpc(int index)
    {
        //Set position to store point
        GameObject[] storePoints = GameObject.FindGameObjectsWithTag("StorePoint");
        GetComponent<testController>().enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.position = storePoints[index].transform.position;
    }

    [ServerRpc]
    private void increasePointCountServerRpc()
    {
        foreach (GameObject pin in GameObject.FindGameObjectsWithTag("Pin"))
        {
            if (pin.GetComponent<PinCollision>().getPinState() && pin.GetComponent<PinCollision>().wasHitOnce == false)
            {
                pin.GetComponent<PinCollision>().wasHitOnce = true;
                gameObject.GetComponent<MP_PlayerAttributes>().points.Value++;
            }
        }
    }
}