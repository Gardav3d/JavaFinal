using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Transform respawnPoint;

    [SerializeField] private MonoBehaviour[] componentsToToggle;

    private int currentHealth;

    private bool isDead;

    public override void OnStartServer()
    {
        base.OnStartServer();
        currentHealth = maxHealth;
        isDead = false; // Initialize on server
    }

    [Server]
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took damage. Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [Server]
    private void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} died.");

        SetPlayerActive(false);
        RpcSetDeadState(true);
        RpcShowDeathUI(Owner);
    }

    [Server]
    public void Respawn()
    {
        Debug.Log($"{gameObject.name} is respawning.");

        isDead = false;
        currentHealth = maxHealth;

        transform.position = respawnPoint.position;

        SetPlayerActive(true);
        RpcSetDeadState(false);
        RpcHideDeathUI(Owner);
    }

    [ObserversRpc]
    private void RpcSetDeadState(bool dead)
    {
        isDead = dead;
    }

    [ObserversRpc]
    private void SetPlayerActive(bool isActive)
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = isActive;

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = isActive;

        foreach (var comp in componentsToToggle)
        {
            if (comp != null)
                comp.enabled = isActive;
        }
    }

    [TargetRpc]
    private void RpcShowDeathUI(NetworkConnection conn)
    {
        PlayerUI.Instance?.ShowDeathUI();
    }

    [TargetRpc]
    private void RpcHideDeathUI(NetworkConnection conn)
    {
        PlayerUI.Instance?.HideDeathUI();
    }
}
