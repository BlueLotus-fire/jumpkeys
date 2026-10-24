
using UnityEngine;
using UnityEngine.InputSystem;

public class BuddyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float minX = -1f;
    [SerializeField] private float maxX = 1f;
    [SerializeField] private float minY = -0.8f;
    [SerializeField] private float maxY = -0.3f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float doubleJumpHeight = 2.5f;
    [SerializeField] private float jumpDuration = 0.8f;

    
    
    private Rigidbody2D rb;
    private Transform body;
    private Animator animator;
    private Vector2 moveInput;

    
    
    
    
    private Vector3 bodyDefaultPosition;
    private float jumpTimer;
    private float jumpStartHeight;
    private float targetHeight;
    private int jumpsUsed;

    public bool IsGrounded = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        body = transform.Find("Sprite");
        animator = body.GetComponent<Animator>();
        bodyDefaultPosition = body.localPosition;
    }

    private void Update()
    {
        ReadMovementInput();
        ReadJumpInput();
        UpdateJump();

        animator.SetBool("IsWalking", moveInput != Vector2.zero);
    }

    private void ReadMovementInput()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.aKey.isPressed)
            moveInput.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            moveInput.x += 1;

        if (Keyboard.current.sKey.isPressed)
            moveInput.y -= 1;

        if (Keyboard.current.wKey.isPressed)
            moveInput.y += 1;

        moveInput = moveInput.normalized;
    }

    private void ReadJumpInput()
    {
        if (Keyboard.current == null)
            return;

        if (!Keyboard.current.spaceKey.wasPressedThisFrame)
            return;

        if (jumpsUsed >= 2)
            return;

        jumpStartHeight = body.localPosition.y - bodyDefaultPosition.y;

        jumpsUsed++;
        targetHeight = jumpsUsed == 1 ? jumpHeight : doubleJumpHeight;

        jumpTimer = 0f;
        IsGrounded = false;
    }

    private void UpdateJump()
    {
        if (IsGrounded)
            return;

        jumpTimer += Time.deltaTime;

        float ascentDuration = jumpDuration * 0.5f;
        float descentDuration = jumpDuration * 0.5f;
        float height;

        if (jumpTimer < ascentDuration)
        {
            float progress = jumpTimer / ascentDuration;
            float easedProgress = Mathf.Sin(progress * Mathf.PI * 0.5f);

            height = Mathf.Lerp(jumpStartHeight, targetHeight, easedProgress);
        }
        else
        {
            float progress = (jumpTimer - ascentDuration) / descentDuration;
            float easedProgress = progress * progress;

            height = Mathf.Lerp(targetHeight, 0f, easedProgress);
        }

        body.localPosition = bodyDefaultPosition + Vector3.up * height;

        float currentHeight = body.localPosition.y - bodyDefaultPosition.y;

        if (jumpTimer >= ascentDuration && currentHeight <= 0.001f)
        {
            body.localPosition = bodyDefaultPosition;
            IsGrounded = true;
            jumpsUsed = 0;
        }
    }

    private void FixedUpdate()
    {
        Vector2 nextPosition =
            rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        nextPosition.x = Mathf.Clamp(nextPosition.x, minX, maxX);
        nextPosition.y = Mathf.Clamp(nextPosition.y, minY, maxY);

        rb.MovePosition(nextPosition);
    }
}
