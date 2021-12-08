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

    // Update is called once per frame
    void Update()
    {
        if(currentHp.Value <= 0)
        {
            RespawnPlayerServerRpc();
            ResetPlayerClientRpc();
            if(IsOwner)
            {
                Debug.LogWarning("You have died");
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Pin") && IsOwner)
        {
            foreach(GameObject pin in GameObject.FindGameObjectsWithTag("Pin"))
            {
                if(pin.GetComponent<PinCollision>().getPinState() && pin.GetComponent<PinCollision>().wasHitOnce == false)
                {
                    pin.GetComponent<PinCollision>().wasHitOnce = true;
                    increasePointCountServerRpc();
                }
            }
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
    private void ResetPlayerClientRpc()
    {
        //Set position to spawn point
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");

        GetComponent<CharacterController>().enabled = false;
        transform.position = spawnPoint.transform.position;
        GetComponent<CharacterController>().enabled = true;
    }

    [ServerRpc]
    private void increasePointCountServerRpc()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject playerObject in players)
        {
            Debug.LogWarning("PIN TRACKED");
            playerObject.GetComponent<MP_PlayerAttributes>().points.Value++;
            //if (playerObject.GetComponent<NetworkObject>().OwnerClientId == spawnerPlayerId)
            //{
                
            //}
        }
    }
}
