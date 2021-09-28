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
        if(Input.GetButtonDown("Fire1") && IsLocalPlayer)
        {
            FireServerRpc();
        }
    }
    [ServerRpc]
    private void FireServerRpc()
    {
        Debug.Log("Fired weapon");
        Rigidbody bulletClone = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        bulletClone.velocity = transform.forward * bulletSpeed;
        bulletClone.gameObject.GetComponent<NetworkObject>().Spawn();
        Destroy(bulletClone.gameObject, 3);
    }
}
