using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class BowAttack : NetworkBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float fireCooldown = 1f;

    private float lastFireTime;

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetButtonDown("Fire1") && Time.time > lastFireTime + fireCooldown)
        {
            lastFireTime = Time.time;
            ShootArrowServer(arrowSpawnPoint.position, arrowSpawnPoint.forward);
        }
    }

    [ServerRpc]
    private void ShootArrowServer(Vector3 position, Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject arrowInstance = Instantiate(arrowPrefab, position, rotation);
        Spawn(arrowInstance);

        ArrowProjectile arrow = arrowInstance.GetComponent<ArrowProjectile>();
        if (arrow != null)
        {
            arrow.Initialize(direction);
        }
    }
}
