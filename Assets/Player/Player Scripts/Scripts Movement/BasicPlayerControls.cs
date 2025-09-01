using UnityEngine;

public class BasicPlayerControls : MonoBehaviour
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
            groundCheck.localPosition = new Vector3(0, -cc.height / 2f, 0);
        }

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        GroundCheck();
        Move();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f; // small downward to keep player grounded
    }

    void Move()
    {
        float speed = walkSpeed;

        // Only allow A and D for horizontal movement
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);

        animator.SetBool("leftPressed", leftPressed);
        animator.SetBool("rightPressed", rightPressed);

        float h = 0f;
        if (leftPressed && !rightPressed) h = -1f;
        else if (rightPressed && !leftPressed) h = 1f;

        if (h != 0)
        {
            // Look in the direction of movement
             Vector3 localScale = transform.localScale;
             localScale.x = h > 0 ? 1 : -1;
             transform.localScale = localScale;
        }
        
        
        
        Vector3 move = transform.right * h;
        cc.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    public void SetWeight(float w)
    {
        currentWeight = w;
    }

    public void AddWeight(float w)
    {
        currentWeight += w;
    }
    
    public void RemoveWeight(float w)
    {
        currentWeight -= w;
    }
}
