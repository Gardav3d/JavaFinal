using FishNet.Object;
using UnityEngine;

public class LocalPlayerCameraActivator : NetworkBehaviour
{
    public GameObject cameraHolder;

    public override void OnStartClient()
    {
        if (IsOwner && cameraHolder != null)
        {
            cameraHolder.SetActive(true);
        }
        else if (cameraHolder != null)
        {
            cameraHolder.SetActive(false);
        }
    }
}
