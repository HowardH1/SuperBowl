using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MP_ChatUI : NetworkBehaviour
{
    public Text chatText = null;
    public InputField chatInput = null;

    NetworkVariableString messages = new NetworkVariableString("Temp");

    public NetworkList<MP_PlayerInfo> chatPlayers;
    private string playerName = "N/A";
    public GameObject scoreCardPanel;
    public Text scoreplayerName;
    public Text scoreKills;
    public Text scoreDeaths;
    private bool showScore = false;

    // Start is called before the first frame update
    void Start()
    {
        messages.OnValueChanged += updateUIClientRpc;
        foreach(MP_PlayerInfo player in chatPlayers)
        {
            if(NetworkManager.LocalClientId == player.networkClientID)
            {        
                playerName = player.networkPlayerName;
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            //show score ui
            showScore = true;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            //hide score ui
            showScore = false;
        }
        if(showScore)
        {
            scoreCardPanel.SetActive(showScore);
            if(IsOwner)
            {
                updateUIScoreServerRpc();
            }
        }
        else
        {
            scoreCardPanel.SetActive(showScore);
        }
    }

    public void handleSend()
    {
        if(!IsServer)
        {
            sendMessageServerRpc(chatInput.text);
        }
        else
        {
            messages.Value += "\n" + playerName + " says: " + chatInput.text;
        }
    }

    [ClientRpc]
    private void updateUIClientRpc(string previousValue, string newValue)
    {
        chatText.text += newValue.Substring(previousValue.Length, newValue.Length - previousValue.Length);
    }

    [ServerRpc(RequireOwnership = false)]
    private void sendMessageServerRpc(string text, ServerRpcParams svrParam = default)
    {
        foreach (MP_PlayerInfo player in chatPlayers)
        {
            if (svrParam.Receive.SenderClientId == player.networkClientID)
            {
                playerName = player.networkPlayerName;
            }
        }
        messages.Value += "\n" + playerName + " says: " + text;
    }

    [ServerRpc]
    private void updateUIScoreServerRpc(ServerRpcParams svrParam = default)
    {
        //clear out old scores
        clearUIScoreClientRpc();
        //get player info
        GameObject[] currentPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject playerObject in currentPlayers)
        {
            foreach(MP_PlayerInfo playerInfo in chatPlayers)
            {
                if(playerObject.GetComponent<NetworkObject>().OwnerClientId == playerInfo.networkClientID)
                {
                    updateUIScoreClientRpc(playerInfo.networkPlayerName, playerObject.GetComponent<MP_PlayerAttributes>().points.Value, playerObject.GetComponent<MP_PlayerAttributes>().deaths.Value);
                }
            }
        }
    }

    [ClientRpc]
    private void updateUIScoreClientRpc(string networkPlayerName, int kills, int deaths)
    {
        if (IsOwner)
        {
            scoreplayerName.text += networkPlayerName + "\n";
            scoreKills.text += kills + "\n";
            scoreDeaths.text += deaths + "\n";
        }
    }

    [ClientRpc]
    private void clearUIScoreClientRpc()
    {
        if(IsOwner)
        {
            scoreplayerName.text = "";
            scoreKills.text = "";
            scoreDeaths.text = "";
        }
    }
}
