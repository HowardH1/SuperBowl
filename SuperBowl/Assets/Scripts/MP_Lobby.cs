using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine.UI;
using MLAPI.NetworkVariable.Collections;
using System;
using MLAPI.Connection;
using MLAPI.SceneManagement;
using MLAPI.Messaging;

public class MP_Lobby : NetworkBehaviour
{
    [SerializeField] private LobbyPanel[] lobbyPlayers;
    [SerializeField] private GameObject playerPrefab;

    private NetworkList<MP_PlayerInfo> nwPlayers = new NetworkList<MP_PlayerInfo>();
    public Button startGameButton;
    void Start()
    {
        UpdateConnListServerRpc(NetworkManager.LocalClientId);
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    public override void NetworkStart()
    {
        Debug.Log("Starting Server");
        if(IsClient)
        {
            nwPlayers.OnListChanged += PlayersInfoChanged;
        }
        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += ClientConnectedHandle;
            NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisconnectedHandle;
            foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                ClientConnectedHandle(client.ClientId);
            }
        }
    }

    private void OnDestroy()
    {
        nwPlayers.OnListChanged -= PlayersInfoChanged;
        if(NetworkManager.Singleton)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= ClientConnectedHandle;
            NetworkManager.Singleton.OnClientDisconnectCallback -= ClientDisconnectedHandle;
        }
    }

    private void PlayersInfoChanged(NetworkListEvent<MP_PlayerInfo> changeEvent)
    {
        int index = 0;
        foreach (MP_PlayerInfo connectedplayer in nwPlayers)
        {
            lobbyPlayers[index].playerName.text = connectedplayer.networkPlayerName;
            lobbyPlayers[index].readyIcon.SetIsOnWithoutNotify(connectedplayer.networkPlayerReady);
            index++;
        }

        if(IsHost)
        {
            startGameButton.gameObject.SetActive(true);
            startGameButton.interactable = CheckEverybodyReady();
        }
    }

    public void StartGame()
    {
        Debug.Log("START");
        if (IsServer)
        { 
            foreach(MP_PlayerInfo tmpClient in nwPlayers)
            {
                GameObject playerSpawn = Instantiate(playerPrefab, new Vector3(2f, 1f, 7f), Quaternion.identity);
                playerSpawn.GetComponent<NetworkObject>().SpawnWithOwnership(tmpClient.networkClientID);
                Debug.Log("PLAYER SPAWNED FOR: " + tmpClient.networkPlayerName);
            }
            NetworkSceneManager.SwitchScene("Tutorial");
        }
        else
        {
            Debug.Log("YOU ARE NOT THE HOST");
        }
    }

    private bool CheckEverybodyReady()
    {
        foreach(MP_PlayerInfo player in nwPlayers)
        {
            if(!player.networkPlayerReady)
            {
                return false;
            }
        }
        return true;
    }

    private void HandleClientConnected(ulong clientId)
    {
        UpdateConnListServerRpc(clientId);
        Debug.Log("A PLAYER HAS CONNECTED ID: " + clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ReadyUpServerRpc(ServerRpcParams serverRpcParams = default)
    {
        for(int indx = 0; indx < nwPlayers.Count; indx++)
        {
            if(nwPlayers[indx].networkClientID == serverRpcParams.Receive.SenderClientId)
            {
                Debug.Log("Updated with new");
                nwPlayers[indx] = new MP_PlayerInfo(nwPlayers[indx].networkClientID, nwPlayers[indx].networkPlayerName, !nwPlayers[indx].networkPlayerReady);
            }
        }
    }

    public void ReadyButtonPressed()
    {
        ReadyUpServerRpc();
    }

    [ServerRpc]
    private void UpdateConnListServerRpc(ulong clientId)
    {
        nwPlayers.Add(new MP_PlayerInfo(clientId, PlayerPrefs.GetString("PName"), false));
    }

    private void ClientDisconnectedHandle(ulong clientId) 
    {
        Debug.Log("TODO: PLAYER DISCONNECTED");
    }

    private void ClientConnectedHandle(ulong clientId)
    {
        Debug.Log("TODO: PLAYER CONNECTED");
    }
}
