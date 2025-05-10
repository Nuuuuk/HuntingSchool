using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 55f;
    public Transform groundCheckPoint;
    public LayerMask groundLayer;

    public bool isGrounded;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpPressed;

    private int groundCount;

    public Vector3 velocity;

    public bool isClimbing;
    public bool canClimb;
    public bool isCrawl;
    public Collider selfCollider;
    public Animator animator;

    public AudioSource audioSource; // import AudioSource
    public AudioClip footstepSound; // footstep audio

    private float stepTimer = 0.5f;  
    private float stepCooldown = 0.5f; 
    private bool isWalking;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        selfCollider = GetComponent<Collider>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();  // get AudioSource component
    }

    private void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        jumpPressed = Input.GetKey(KeyCode.Space);

        if (canClimb && !isClimbing)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                StartClimbing();
            }
        }

        if (isClimbing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpOut();
            }
        }

        ToggleCrawl();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y <= 0)
        {
            CheckGrounded();
        }

        MovePlayer();
        Jump();

        rb.velocity = velocity;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Net"))
        {
            canClimb = true;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Dead();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            var currentPlatform = collision.gameObject.GetComponent<MovingPlatform>();
            if (currentPlatform != null)
            {
                // 手动同步位置差
                rb.position += currentPlatform.deltaPosition;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Net"))
        {
            canClimb = false;
            StopClimbing();
        }
    }

    void MovePlayer()
    {
        float horizontalInput = moveInput.x;
        float verticalInput = moveInput.y;

        if (!isClimbing)
        {
            Vector3 moveDir = (transform.forward * horizontalInput - transform.right * verticalInput).normalized;
            float speed = moveSpeed;
            if (!isGrounded)
            {
                speed *= 0.5f;
            }

            velocity = moveDir * speed;
            velocity.y = rb.velocity.y;

            if (moveDir.magnitude > 0)
                animator.transform.forward = moveDir;
            animator.SetFloat("velocity", moveDir.magnitude);
            animator.SetFloat("velocityY", velocity.y);
        }
        else
        {
            Vector3 moveDir = transform.up * verticalInput;
            float speed = moveSpeed;
            velocity = moveDir * speed;
        }
    }

    void Jump()
    {
        if (!isCrawl && isGrounded && jumpPressed)
        {
            animator.SetTrigger("Jump");
            // Debug.Log("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpPressed = false;
        }
    }

    void PlayFootstepSound()
    {
        // play footstep audio
        if (footstepSound != null && audioSource != null)
        {
            audioSource.clip = footstepSound;
            audioSource.Play();
        }
    }

    void StopFootstepSound()
    {
        // stop footstep audio
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void ToggleCrawl()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isCrawl)
            {
                bool canResume = true;
                var colli = Physics.OverlapSphere(transform.position, 1, ~groundLayer);
                foreach (var item in colli)
                {
                    if (item.CompareTag("Shelf"))
                    {
                        canResume = false;
                        break;
                    }
                }
                if (!canResume)
                {
                    return;
                }
                //transform.localScale = new Vector3(1, 1, 1);
                isCrawl = false;
            }
            else
            {
                //transform.localScale = new Vector3(1, 0.5f, 1);
                isCrawl = true;
            }
            animator.SetBool("isCrouch", isCrawl);
        }
    }

    public Collider[] cols = new Collider[2];
    void CheckGrounded()
    {
        isGrounded = false;
        int count = Physics.OverlapBoxNonAlloc(groundCheckPoint.position, new Vector3(.5f, .1f, .5f), cols);
        for (int i = 0; i < count; i++)
        {
            if (cols[i] != selfCollider)
            {
                isGrounded = true;
                break;
            }
        }
        animator.SetBool("isGround", isGrounded);
        //isGrounded = Physics.OverlapBox(, Quaternion.identity, groundLayer).Length > 0;
    }

    void Dead()
    {
        GameMgr.Instance.RestartLevel();
    }

    void StartClimbing()
    {
        isClimbing = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    void StopClimbing()
    {
        isClimbing = false;
        rb.useGravity = true;
    }

    void JumpOut()
    {
        rb.useGravity = true;
        rb.AddForce(-transform.forward * 500, ForceMode.Impulse);
    }

    public void SetRotationY(float rotationY)
    {
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    public void Reset(Vector3 spawnPos)
    {
        //transform.localScale = new Vector3(1, 1, 1);
        transform.position = spawnPos;
        isCrawl = false;
        animator.enabled = false;
        animator.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheckPoint.position, new Vector3(.5f, .1f, .5f));
    }
}