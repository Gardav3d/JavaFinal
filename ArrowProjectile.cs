using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class ArrowProjectile : NetworkBehaviour
{
    [SerializeField] private float speed = 25f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float lifetime = 5f;

    private Vector3 direction;

    public void Initialize(Vector3 dir)
    {
        direction = dir.normalized;
        transform.rotation = Quaternion.LookRotation(direction); // 👈 Add this
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return; // ✅ Must run only on server

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null && !health.IsOwner)
        {
            health.TakeDamage(damage);
        }

        // Destroy on server so it propagates to clients
        Despawn();
    }

    private NetworkObject owner;

    public void SetOwner(NetworkObject ownerObj)
    {
        owner = ownerObj;
    }

}
