using FishNet.Managing;
using UnityEngine;

public class ServerLauncher : MonoBehaviour
{
    public NetworkManager networkManager;

    void Start()
    {
        // Check command-line arguments
        string[] args = System.Environment.GetCommandLineArgs();
        bool isServerMode = Application.isBatchMode || System.Array.Exists(args, arg => arg == "-server");

        if (isServerMode)
        {
            Debug.Log("Starting in server mode...");
            networkManager.ServerManager.StartConnection(); // Start FishNet server
        }
    }
}
