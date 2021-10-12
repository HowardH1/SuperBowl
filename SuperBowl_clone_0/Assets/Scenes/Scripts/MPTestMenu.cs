using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class MPTestMenu : MonoBehaviour
{
    public GameObject netMenu;

    public void HostClicked()
    {
        NetworkManager.Singleton.StartHost();
        netMenu.SetActive(false);
    }

    public void ClientClicked()
    {
        NetworkManager.Singleton.StartClient();
        netMenu.SetActive(false);
    }
}
