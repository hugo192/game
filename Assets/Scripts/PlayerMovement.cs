using UnityEngine;

public class JumpKingController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;

    [Header("Jump Settings")]
    public float minJumpForce = 5f;
    public float maxJumpForce = 20f;
    public float chargeRate = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    private bool isGrounded;
    private bool isCharging;
    private float jumpPower;
    private int jumpDirection; // -1 = left, 0 = up, 1 = right
    private int facingDir = 1; // -1 = left, 1 = right

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // --- Ground check ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // --- Movement (walking only if grounded & not charging) ---
        float move = Input.GetAxisRaw("Horizontal");

        if (isGrounded && !isCharging)
        {
            if (move != 0)
            {
                rb.linearVelocity = new Vector2(move * walkSpeed, rb.linearVelocity.y);
                facingDir = (int)Mathf.Sign(move);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // stop instantly
            }
        }

        // --- Start charging jump ---
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            jumpPower = minJumpForce;
        }

        // --- Charging ---
        if (isCharging && Input.GetKey(KeyCode.Space))
        {
            jumpPower += chargeRate * Time.deltaTime;
            jumpPower = Mathf.Clamp(jumpPower, minJumpForce, maxJumpForce);

            // Choose jump direction while charging
            if (Input.GetKey(KeyCode.A))
            {
                jumpDirection = -1;
                facingDir = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                jumpDirection = 1;
                facingDir = 1;
            }
            else
            {
                jumpDirection = 0; // straight up
            }
        }

        // --- Release jump ---
        if (isCharging && Input.GetKeyUp(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(jumpDirection * walkSpeed, jumpPower);
            isCharging = false;
        }

        // --- Animation hooks (currently do nothing) ---
        // Later you can add:
        // UpdateAnimation(isGrounded, isCharging, facingDir);
    }

    // Placeholder function for animation integration
    private void UpdateAnimation(bool grounded, bool charging, int direction)
    {
        // Example for later:
        // anim.SetBool("IsGrounded", grounded);
        // anim.SetBool("IsCharging", charging);
        // anim.SetInteger("FacingDir", direction);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
