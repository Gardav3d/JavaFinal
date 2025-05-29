using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerUI : NetworkBehaviour
{
    public static PlayerUI Instance;

    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Button respawnButton;

    private PlayerHealth localPlayerHealth;

    private void Awake()
    {
        Instance = this;
    }

    public void SetPlayerHealth(PlayerHealth health)
    {
        localPlayerHealth = health;
    }

    public void ShowDeathUI()
    {
        deathPanel.SetActive(true);
    }

    public void HideDeathUI()
    {
        deathPanel.SetActive(false);
    }

    private void Start()
    {
        respawnButton.onClick.AddListener(OnRespawnButton);
        deathPanel.SetActive(false);
    }

    private void OnRespawnButton()
    {
        if (localPlayerHealth != null && localPlayerHealth.IsOwner)
        {
            RequestRespawn();
        }
    }

    [ServerRpc]
    private void RequestRespawn()
    {
        localPlayerHealth.Respawn();
    }
}
