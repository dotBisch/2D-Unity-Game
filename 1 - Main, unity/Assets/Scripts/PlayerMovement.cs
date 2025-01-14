using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyManager km;

    private float horizontal;
    private float speed = 4f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    private float jumpTimeCounter;
    private float maxJumpTime = 0.05f;

    private Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpTimeCounter = maxJumpTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.y, jumpingPower);
        }

        if (Input.GetButton("Jump") && jumpTimeCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.y, jumpingPower);
            jumpTimeCounter -= Time.deltaTime;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumpTimeCounter = 0;
        }

        if (IsGrounded())
        {
            animator.SetBool("isJumping", false);
        }

        Flip();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            Destroy(other.gameObject);
            km.keyCount++;
        }
        if (other.CompareTag("Portal") && km.keyCount == 5)
        {
                SceneController.instance.NextLevel();
        }
    }
}
