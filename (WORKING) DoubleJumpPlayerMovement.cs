using UnityEngine;
using FishNet.Object;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public Transform cameraTransform;

    private CharacterController controller;
    private float verticalVelocity;
    private bool isGrounded;

    private int jumpCount = 0;
    public int maxJumps = 2; // Set to 2 for double jump

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            verticalVelocity = -2f; // Keeps player grounded
            jumpCount = 0; // Reset jump count when grounded
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Jump input
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
        }

        // Apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        // Camera-relative movement
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * moveZ + camRight * moveX;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime * moveSpeed);
    }
}
