using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem smokeFX;
    [SerializeField] private BoxCollider2D playerCollider;
    private bool isFacingRight = true;

    [Space]
    [Header("Player Movement")]
    [SerializeField] private float speed = 5f;
    float horizontalMovement;

    [Space]
    [Header("Player Jumping")]
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private int maxJumps = 2;
    private int jumpRemaining;

    [Space]
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private Vector2 groundCheckSize = new(0.5f, 0.5f);
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    private bool isOnPlatform;

    [Space]
    [Header("Wall Check")]
    [SerializeField] private Transform wallCheckPosition;
    [SerializeField] private Vector2 wallCheckSize = new(0.5f, 0.5f);
    [SerializeField] private LayerMask wallLayer;

    [Space]
    [Header("Gravity")]
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private float fallSpeedMultiplier = 2f;

    [Space]
    [Header("Wall Movement")]
    [SerializeField] private float wallSlideSpeed = 2;
    private bool isWallSliding;

    [Space]
    [SerializeField] private Vector2 wallJumpPower = new(5f, 10f);
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.5f;
    private float wallJumpTimer;

    void Update()
    {
        GroundCheck();
        ProcessGravity();
        ProcessWallSlide();
        ProcessWallJump();

        if (!isWallJumping)
        {
            rigidBody.velocity = new Vector2(horizontalMovement * speed, rigidBody.velocity.y);
            Flip();
        }

        animator.SetInteger("doubleJump", jumpRemaining);
        animator.SetFloat("yVelocity", rigidBody.velocity.y);
        animator.SetFloat("magnitude", rigidBody.velocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.action.ReadValue<Vector2>().x;
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0
            || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

            if (rigidBody.velocity.y == 0)
                smokeFX.Play();
        }

    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded &&
            isOnPlatform && playerCollider.enabled)
            StartCoroutine(DisablePlayerCollider(0.25f));
    }

    private IEnumerator DisablePlayerCollider(float disableTime)
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);
        playerCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            isOnPlatform = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            isOnPlatform = false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpRemaining > 0)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPower);
            jumpRemaining--;
            JumpFX();
        }

        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rigidBody.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y); ;
            wallJumpTimer = 0;

            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
        }
    }

    private void JumpFX()
    {
        smokeFX.Play();
        animator.SetTrigger("jump");
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0, groundLayer))
        {
            jumpRemaining = maxJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPosition.position, wallCheckSize, 0, wallLayer);
    }

    private void ProcessGravity()
    {
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.gravityScale = baseGravity * fallSpeedMultiplier;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Max(rigidBody.velocity.y, -maxFallSpeed));
        }
        else
        {
            rigidBody.gravityScale = baseGravity;
        }

    }

    private void ProcessWallSlide()
    {
        if (!isGrounded & WallCheck() & horizontalMovement != 0)
        {
            isWallSliding = true;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Max(rigidBody.velocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPosition.position, wallCheckSize);
    }
}