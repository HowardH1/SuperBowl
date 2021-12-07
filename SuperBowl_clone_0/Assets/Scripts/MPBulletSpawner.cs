using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class MPBulletSpawner : NetworkBehaviour
{
    public Rigidbody bullet;
    public Transform bulletPos;
    private float bulletSpeed = 10f;

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && IsOwner)
        {
            FireServerRpc(gameObject.GetComponent<MP_PlayerAttributes>().powerUp.Value);
        }
    }
    [ServerRpc]
    private void FireServerRpc(bool powerUp, ServerRpcParams serverRpcParams = default)
    {
        Debug.LogWarning("Fired weapon");
        Rigidbody bulletClone = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletClone.velocity = transform.forward * bulletSpeed;
        bulletClone.GetComponent<MP_BulletScript>().spawnerPlayerId = serverRpcParams.Receive.SenderClientId;
        bulletClone.gameObject.GetComponent<NetworkObject>().Spawn();
        Destroy(bulletClone.gameObject, 3);
        if(powerUp)
        {
            Vector3 temp = new Vector3(1, 0, 0);
            bulletPos.Translate(temp, bulletPos);
            bulletClone = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
            bulletClone.velocity = transform.forward * bulletSpeed;
            bulletClone.GetComponent<MP_BulletScript>().spawnerPlayerId = serverRpcParams.Receive.SenderClientId;
            bulletClone.gameObject.GetComponent<NetworkObject>().Spawn();
            Destroy(bulletClone.gameObject, 3);

            temp = new Vector3(-2, 0, 0);
            bulletPos.Translate(temp, bulletPos);
            bulletClone = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
            bulletClone.velocity = transform.forward * bulletSpeed;
            bulletClone.GetComponent<MP_BulletScript>().spawnerPlayerId = serverRpcParams.Receive.SenderClientId;
            bulletClone.gameObject.GetComponent<NetworkObject>().Spawn();
            Destroy(bulletClone.gameObject, 3);

            temp = new Vector3(1, 0, 0);
            bulletPos.Translate(temp, bulletPos);
        }
    }
}
