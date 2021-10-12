using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.SceneManagement;

public class MainMenu : NetworkBehaviour
{
    [SerializeField] private InputField playerName;
    public void hostGame()
    {
        PlayerPrefs.SetString("PName", playerName.text);
        NetworkManager.Singleton.StartHost();
        NetworkSceneManager.SwitchScene("Lobby");
    }

    public void joinGame()
    {
        PlayerPrefs.SetString("PName", playerName.text);
        NetworkManager.Singleton.StartClient();
        Debug.Log("Client Started");
    }

    /*public void quitGame()
    {
        Debug.Log("Game Ended");
        Application.Quit();
    }*/
}
