using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class CharacterSelection : NetworkBehaviour
{
    [SerializeField] private List<GameObject> chars = new List<GameObject>();
    [SerializeField] private GameObject characterSelectorPanel;
    [SerializeField] private GameObject canvasObject;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
            canvasObject.SetActive(false);
    }

    public void SpawnWarrior()
    {
        characterSelectorPanel.SetActive(false);
        Spawn(0, LocalConnection);
    }

    public void SpawnArcher()
    {
        characterSelectorPanel.SetActive(false);
        Spawn(1, LocalConnection);
    }

    public void SpawnMage()
    {
        characterSelectorPanel.SetActive(false);
        Spawn(2, LocalConnection);
    }

    [ServerRpc(RequireOwnership = false)]

    void Spawn(int spawnIndex, NetworkConnection conn)
    {
        GameObject player = Instantiate(chars[spawnIndex], SpawnPoint.instance.transform.position, quaternion.identity);
        Spawn(player, conn);
    }
}
