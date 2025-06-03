using UnityEngine;
using FishNet.Managing;
using UnityEngine.UI;
using TMPro;

public class ConnectionManager : MonoBehaviour
{
    public NetworkManager networkManager;
    public TMP_InputField ipInputField;

    public void HostGame()
    {
        networkManager.ServerManager.StartConnection();
        networkManager.ClientManager.StartConnection();
        Debug.Log("Hosting game...");
    }

    public void JoinGame()
    {
        string ip = ipInputField.text.Trim();

        if (!string.IsNullOrEmpty(ip))
        {
            networkManager.TransportManager.Transport.SetClientAddress(ip);
        }

        networkManager.ClientManager.StartConnection();
    }

}
