using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private Rigidbody rb;

    private bool isGrounded;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!IsOwner) return;

       OnMove();
    }

    private void OnMove()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Debug.Log($"Move input: {moveInput}");

        if (moveInput != 0)
        {
            Debug.Log("player is moving");
        }

        rb.velocity = new Vector3(moveInput * speed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("player is trying to jump");

            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            isGrounded = false;
            SubmitJumpRequestServerRpc();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; 
        }
    }

    [ServerRpc]
    void SubmitJumpRequestServerRpc()
    {
        JumpClientRpc();
    }

    [ClientRpc]
    void JumpClientRpc()
    {
        if (!IsOwner)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

}
