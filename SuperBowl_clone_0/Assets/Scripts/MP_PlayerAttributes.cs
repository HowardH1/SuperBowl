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
    public Slider hpBar;
    private float maxHp = 100f;
    private float damageVal = 20f;
    private float healVal = 25f;
    public NetworkVariableBool powerUp = new NetworkVariableBool(false);
    private NetworkVariableFloat currentHp = new NetworkVariableFloat(100f);

    public NetworkVariableInt kills = new NetworkVariableInt(0);
    public NetworkVariableInt deaths = new NetworkVariableInt(0);
    // Update is called once per frame
    void Update()
    {
        hpBar.value = currentHp.Value / maxHp;
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
        if(collision.gameObject.CompareTag("Bullet") && IsOwner)
        {
            if(collision.gameObject.GetComponent<MP_BulletScript>().spawnerPlayerId != OwnerClientId)
            {
                if(currentHp.Value - damageVal <= 0)
                {
                    increaseKillCountServerRpc(collision.gameObject.GetComponent<MP_BulletScript>().spawnerPlayerId);
                }
                TakeDamageServerRpc(damageVal);
                Destroy(collision.gameObject);
            }
        }
        else if(collision.gameObject.CompareTag("MedKit") && IsOwner)
        {
            HealDamageServerRpc(healVal);
        }
        else if (collision.gameObject.CompareTag("PowerUp") && IsOwner)
        {
            damageVal = damageVal / 2;
            powerUp.Value = true;
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
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        //get random spawn point location
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        int index = UnityEngine.Random.Range(0, spawnPoints.Length);

        GetComponent<CharacterController>().enabled = false;
        transform.position = spawnPoints[index].transform.position;
        GetComponent<CharacterController>().enabled = true;
    }

    [ServerRpc]
    private void increaseKillCountServerRpc(ulong spawnerPlayerId)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject playerObject in players)
        {
            if(playerObject.GetComponent<NetworkObject>().OwnerClientId == spawnerPlayerId)
            {
                Debug.LogWarning("KILL TRACKED");
                playerObject.GetComponent<MP_PlayerAttributes>().kills.Value++;
            }
        }
    }
}
