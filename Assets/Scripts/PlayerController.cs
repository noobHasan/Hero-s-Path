using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimator;
    public float moveSpeed = 5f;
    private float jumpForce = 6f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        // Animation
        playerAnimator.SetBool("isRunning", moveDirection.magnitude > 0);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnimator.SetBool("isJumping", true);
        }

        // Rotate
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDirection);

        // Save movement input
        _moveDir = moveDirection;
    }

    Vector3 _moveDir;

    private void FixedUpdate()
    {
        // Physics movement — smooth & stable
        Vector3 targetPos = rb.position + _moveDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
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
