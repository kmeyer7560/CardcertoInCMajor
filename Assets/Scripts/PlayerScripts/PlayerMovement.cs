using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float moveSpeed;
    public float activeSpeed;
    public float dashLength = .5f;
    public float dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;
    public Vector2 savedDirection;
    public bool vulnerable;
    public bool moveable;
    public Rigidbody2D rb;
    public Animator animator;
    //public GameObject trail;
    public Transform player;
    bool collided;
    bool inReinDash;
    public LayerMask wallLayer;

    public Vector2 moveDirection;
    public GameObject fireFX;
    public GameObject dashHitBox;
    void Start()
    {
        activeSpeed = moveSpeed;
        vulnerable = true;
        moveable = true;
        //trail.GetComponent<ParticleSystem>().enableEmission = false;
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                activeSpeed = moveSpeed;
                dashCoolCounter = dashCooldown;
                vulnerable = true;
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }

        Move();
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * activeSpeed, moveDirection.y * activeSpeed);
        spriteRenderer.flipX = rb.velocity.x < 0;
    }

    public void dash(float dashSpeed, bool t, bool hitbox)
    {
        Debug.Log("dashed");
        Debug.Log(dashSpeed);
        StartCoroutine(DashCoroutine(dashSpeed));
        if (t)
        {
            //trail.GetComponent<ParticleSystem>().enableEmission = true;
            StartCoroutine(FireDash());
        }
        if (hitbox)
        {
            dashHitBox.SetActive(true);
        }
    }
    private IEnumerator FireDash()
    {
        for(int i=0; i<=10; i++)
        {
            Instantiate(fireFX, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(.01f);
        }
    }
   public void reinDash()
{
    if (moveable && savedDirection != Vector2.zero) // Ensure the player can only dash if they are currently able to move and have a direction
    {
        collided = false;
        StartCoroutine(ReinDashCoroutine());
    }
}

private IEnumerator ReinDashCoroutine()
{
    // Disable normal movement
    moveable = false;
    inReinDash = true;

    float dashDistance = 0.2f; // Use activeSpeed for dash speed
    Vector2 direction = savedDirection.normalized; // Use the saved direction for the dash

    // Define the number of rays and their offsets
    int numberOfRays = 3; // 1 main ray + 1 above + 1 below
    float raySpacing = 3f; // Space between the rays

    while (!collided)
    {
        // Move the player in the direction of the dash
        Vector2 targetPosition = rb.position + direction * dashDistance;

        // Check for collision with the environment using multiple rays
        bool hitDetected = false;
        for (int i = -1; i <= 1; i++) // -1 for below, 0 for main, 1 for above
        {
            Vector2 rayOrigin = rb.position + new Vector2(0, i * raySpacing);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, 0.5f, wallLayer);
            Debug.DrawRay(player.position, direction * 0.5f, Color.red);
            if (hit.collider != null && (!hit.collider.CompareTag("Player")))
            {
                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    hit.transform.gameObject.GetComponent<EnemyHealth>().takeDamage(60);
                }
                hitDetected = true;
                break;
            }
        }

        if (hitDetected)
        {
            // Stop dashing if we hit a wall
            collided = true;
            break;
        }

        rb.MovePosition(targetPosition);
        yield return new WaitForFixedUpdate(); // Wait for the next fixed update
    }

    // Reset the player's state after dashing
    moveable = true; // Re-enable movement
    inReinDash = false;
    rb.velocity = Vector2.zero; // Reset velocity
}

public IEnumerator DashCoroutine(float dashSpeed)
{
    Debug.Log("Started Routine");
    vulnerable = false;
    dashCounter = dashLength;

    float dashDistance = dashSpeed * Time.fixedDeltaTime;
    //trail.SetActive(true);

    int steps = Mathf.CeilToInt(dashLength / Time.fixedDeltaTime);
    Vector2 direction = savedDirection.normalized;

    // Define the number of rays and their offsets
    int numberOfRays = 3; // 1 main ray + 1 above + 1 below
    float raySpacing = 1f; // Space between the rays

    for (int i = 0; i < steps; i++)
    {
        Vector2 targetPosition = rb.position + direction * dashDistance;

        // Check for collision with the environment using multiple rays
        bool hitDetected = false;
        for (int j = -1; j <= 1; j++) // -1 for below, 0 for main, 1 for above
        {
            Vector2 rayOrigin = rb.position + new Vector2(0, j * raySpacing);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, dashDistance, wallLayer);
            if (hit.collider != null && hit.collider.CompareTag("Environment"))
            {
                hitDetected = true;
                break;
            }
        }

        if (hitDetected)
        {
            break; // Stop dashing if we hit a wall
        }

        rb.MovePosition(targetPosition);
        yield return new WaitForFixedUpdate();
    }

    // Reset the player's velocity after dashing
    StartCoroutine(DisableColliderAfterDelay(5f)); // Start the coroutine to disable the collider
    rb.velocity = Vector2.zero;
    dashCoolCounter = dashCooldown;
    //trail.GetComponent<ParticleSystem>().enableEmission = false;
    dashHitBox.SetActive(false);
    vulnerable = true;
}

    public void stupidDumbassFunction()
    {
        StartCoroutine(guitarAnimRoutine());
    }

    public IEnumerator guitarAnimRoutine()
    {
        Debug.Log("Starting guitar animation");
        animator.SetBool("guitar", true);
        yield return new WaitForSeconds(3f);
        Debug.Log("Ending guitar animation");
        animator.SetBool("guitar", false);
    }

    private IEnumerator DisableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
    }

    public void speedUp()
    {
        activeSpeed = 8;
        StartCoroutine(speedupTimer());
    }

    IEnumerator speedupTimer()
    {
        yield return new WaitForSeconds(2f);
        activeSpeed = moveSpeed;
    }

    void Update()
    {
    
        if (moveable)
        {
            ProcessInputs();
        }

        if (savedDirection != moveDirection && moveDirection != Vector2.zero)
        {
            savedDirection = moveDirection;
        }
       
        animator.SetBool("moving", rb.velocity != Vector2.zero);
      
    }
}
