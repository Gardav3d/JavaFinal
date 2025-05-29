using UnityEngine;
using FishNet.Object;

public class PlayerAttack : NetworkBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 0.5f;
    public int damage = 20;
    public float attackRadius = 0.5f;
    public LayerMask attackMask;

    [Header("References")]
    public Camera playerCamera;
    public Animator attackAnimator;

    private float lastAttackTime = 0f;

    private void Update()
    {
        if (!IsOwner || !IsClientInitialized) return;

        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            // Origin from the player's chest or front
            Vector3 origin = transform.position + Vector3.up * 1f;
            Vector3 direction = playerCamera.transform.forward;

            // Debug ray (client-side)
            Debug.DrawRay(origin, direction * attackRange, Color.red, 1f);

            TriggerAttackAnimation();
            Server_PerformAttack(origin, direction);
        }
    }

    private void TriggerAttackAnimation()
    {
        if (attackAnimator != null)
        {
            attackAnimator.Play("SwingAnim", 0, 0f);
        }
    }

    [ServerRpc]
    private void Server_PerformAttack(Vector3 origin, Vector3 direction)
    {
        Debug.Log($"[Server] Performing attack from Player {OwnerId}");

        // Debug ray (server-side)
        Debug.DrawRay(origin, direction * attackRange, Color.green, 1f);

        Observers_PlayAttackAnimation();

        RaycastHit[] hits = Physics.SphereCastAll(origin, attackRadius, direction, attackRange, attackMask);

        if (hits.Length == 0)
        {
            Debug.LogWarning("[Server] SphereCast hit nothing.");
        }

        foreach (RaycastHit hit in hits)
        {
            Debug.Log("[Server] Hit object: " + hit.collider.name);

            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("[Server] Skipped self.");
                continue;
            }

            PlayerHealth target = hit.collider.GetComponent<PlayerHealth>();
            if (target != null)
            {
                Debug.Log($"[Server] Damaging player: {hit.collider.name}");
                target.TakeDamage(damage);
                break;
            }
            else
            {
                Debug.Log("[Server] No PlayerHealth component on: " + hit.collider.name);
            }
        }
    }

    [ObserversRpc]
    private void Observers_PlayAttackAnimation()
    {
        TriggerAttackAnimation();
    }
}
