using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float gravity = -20f;
    public float jumpHeight = 1.6f;

    [Header("Grounded")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundMask;

    [Header("Weight")]
    public float currentWeight = 0f;

    public bool canMove = true;

    private CharacterController cc;
    private Vector3 velocity;
    private bool isGrounded;

    private Animator animator;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (groundCheck == null)
        {
            groundCheck = new GameObject("GroundCheck").transform;
            groundCheck.SetParent(transform);
            
            //Determine how far below the player the groundCheck object should be placed.
            //If the CharacterController (cc) exists and has a valid height, use half of that height.
            //Otherwise, default to 1 unit below the player.
            float halfHeight = (cc != null && cc.height > 0f) ? cc.height / 2f : 1f;
            
            //Position the groundCheck directly below the player, at the bottom of the CharacterController capsule.
            groundCheck.localPosition = new Vector3(0, -halfHeight, 0);
        }

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //Always do grounded check so physics state remains correct.
        GroundCheck();

        //player cant move when platfrom is moving
        if (!canMove)
        {
            if (animator != null)
            {
                animator.SetBool("leftPressed", false);
                animator.SetBool("rightPressed", false);
            }

            velocity.y += gravity * Time.deltaTime;
            cc.Move(velocity * Time.deltaTime);

            return;
        }

        Move();
    }

    void GroundCheck()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;
    }

    void Move()
    {
        float speed = walkSpeed;

        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);

        if (animator != null)
        {
            animator.SetBool("leftPressed", leftPressed);
            animator.SetBool("rightPressed", rightPressed);
        }

        //movement
        float h = 0f;
        if (leftPressed && !rightPressed)
            h = -1f;
        else if (rightPressed && !leftPressed)
            h = 1f;

        if (h != 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = h > 0 ? 1 : -1;
            transform.localScale = localScale;
        }

        Vector3 move = transform.right * h;
        cc.Move(move * (speed * Time.deltaTime));

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        AudioManager.Instance?.SetMovementLoop(h != 0f);
    }

    public void SetWeight(float w)
    {
        currentWeight = w;
    }
}
