using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimator;
    public float moveSpeed = 5f;
    private float jumpForce = 6f;

    private Vector3 targetMoveDir;
    private Vector3 currentMoveDir;
    private Vector3 smoothVelocity;

    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        targetMoveDir = new Vector3(moveX, 0, moveZ).normalized;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnimator.SetBool("isJumping", true);
        }
    }

    private void FixedUpdate()
    {
        currentMoveDir = Vector3.SmoothDamp(currentMoveDir,targetMoveDir,ref smoothVelocity,0.1f);
        playerAnimator.SetBool("isRunning", currentMoveDir.magnitude > 0.1f);

        rb.MovePosition(rb.position + currentMoveDir * moveSpeed * Time.fixedDeltaTime);

        if(currentMoveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(currentMoveDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            playerAnimator.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
