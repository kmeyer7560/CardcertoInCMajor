using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement: MonoBehaviour
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
    public TrailRenderer tr;
     

    public Rigidbody2D rb;
    public Animator animator;

    public Vector2 moveDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        tr.emitting = false;
        activeSpeed = moveSpeed;
        vulnerable = true;
        moveable = true;
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

        if (dashCoolCounter > 0 )
        {
            dashCoolCounter -= Time.deltaTime;
        }

        Move();
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * activeSpeed, moveDirection.y * activeSpeed);
        if (rb.velocity.x < 0)
        {
                spriteRenderer.flipX = true;
        }
        if (rb.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void dash(float dashSpeed, bool trail)
    {
        if (trail)
        {
            tr.emitting = true;
        }
        Debug.Log("dashed");
        Debug.Log(dashSpeed);
        StartCoroutine(DashCoroutine(dashSpeed));
    }

   public IEnumerator DashCoroutine(float dashSpeed)
{
    Debug.Log("Started Routine");
    vulnerable = false;
    activeSpeed = dashSpeed;
    dashCounter = dashLength;

    float dashTime = 0f;
    float dashDistance = dashSpeed * Time.fixedDeltaTime; // Calculate the distance to move each frame

    // Calculate the total number of steps for the dash
    int steps = Mathf.CeilToInt(dashLength / Time.fixedDeltaTime);
    Vector2 direction = savedDirection.normalized;

    for (int i = 0; i < steps; i++)
    {
        // Calculate the target position for this step
        Vector2 targetPosition = rb.position + direction * dashDistance;

        // Check for potential collisions
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, dashDistance);
        if (hit.collider != null && hit.collider.CompareTag("Environment"))
        {
            // If a collision is detected, stop the dash
            break;
        }

        // Move the player
        rb.MovePosition(targetPosition);

        yield return new WaitForFixedUpdate(); // Wait for the next physics update
    }

    // Reset the player's velocity after dashing
    tr.emitting = false;
    rb.velocity = Vector2.zero;
    activeSpeed = moveSpeed;
    dashCoolCounter = dashCooldown;
    vulnerable = true;
}




    // Update is called once per frame
    void Update() 
    {
        if (moveable)
        {
            ProcessInputs();
        }
    
        if(savedDirection!= moveDirection && moveDirection!= Vector2.zero){ //ian coded
            savedDirection = moveDirection; //ian coded
        }

        if (rb.velocity != Vector2.zero)
        {
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
        }
}