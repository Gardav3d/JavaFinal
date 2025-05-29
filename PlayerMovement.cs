using UnityEngine;
using FishNet.Object;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    private CharacterController controller;
    public Transform cameraTransform; // Assign this to the local cameraHolder in the prefab

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Camera-relative directions
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Flatten the camera vectors so movement stays on the ground
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camForward * moveZ + camRight * moveX);
        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}
