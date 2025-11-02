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
    
    //reference to animator component
    private Animator animator;
    //tracks last frames y position
    private float lastY;
    //timer to stop rising and falling animations playing when spawning
    private float vSpeedGraceTimer = 0f;
    private const float vSpeedGraceDuration = 0.25f;
    //tracks whther canmove changed between frames
    private bool lastCanMove;
    [Header("Particles")]
    //reference to the particle system
    public ParticleSystem gasParticles;
    
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
        
        //gets reference to the animator in the players child object
        animator = GetComponentInChildren<Animator>();
        //stores inital position for vertical speed calculation
        lastY = transform.position.y;
        //begins short grace period to stop rising and falling animations to play when player spawns
        vSpeedGraceTimer = vSpeedGraceDuration;
        //tracks intial movement state
        lastCanMove = canMove;
    }

    void Update()
    {
        //if changed from cant move to can move reapply grace period
        if (canMove && !lastCanMove)
        {
            vSpeedGraceTimer = vSpeedGraceDuration;
        }
        lastCanMove = canMove;
        
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
            
            //calculates vertical speed m world postion change
            float v = (transform.position.y - lastY) / Time.deltaTime;
            lastY = transform.position.y;
            
            //applies grace period to stop the rising and falling animations playing at spawn
            if (vSpeedGraceTimer > 0f)
            {
                vSpeedGraceTimer -= Time.deltaTime;
                v = 0f;
            }
            else if (Mathf.Abs(v) < 0.05f)
            {
                v = 0f;
            }
            
            //sends final vspeed value to the animator for the rising and falling
            if (animator != null)
            {
                animator.SetFloat("vSpeed", v);
            }
            return;
        }

        Move();
        
        //calculates vertical speed after moving the frame
        float worldV = (transform.position.y - lastY) / Time.deltaTime;
        lastY = transform.position.y;
        
        //applies the grace period 
        if (vSpeedGraceTimer > 0f)
        {
            vSpeedGraceTimer -= Time.deltaTime;
            worldV = 0f;
        }
        else if (Mathf.Abs(worldV) < 0.05f)
        {
            worldV = 0f;
        }
        
        //sends it to the animator
        if (animator != null)
        {
            animator.SetFloat("vSpeed", worldV);
        }
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
        
        //sends input states to the animator
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
        
        //controls the player movement looping sound
        AudioManager.Instance?.SetMovementLoop(h != 0f);
        
        //plays the gas particles when the player is moving
        if (gasParticles != null)
        {
            //true if moving left or right
            bool isMoving = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f;

            if (isMoving && !gasParticles.isPlaying)
            {
                //emits the gas particles
                gasParticles.Play();
            }
            else if (!isMoving && gasParticles.isPlaying)
            {
                //stops and clears the gas aprticles when player stops moving
                gasParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

    }

    public void SetWeight(float w)
    {
        currentWeight = w;
    }
}
