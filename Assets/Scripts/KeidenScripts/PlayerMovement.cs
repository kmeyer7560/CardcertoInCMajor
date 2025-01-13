using System.Collections;
using System.Collections.Generic;
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
     

    public Rigidbody2D rb;
    public Animator animator;

    public Vector2 moveDirection;
    
    // Start is called before the first frame update
    void Start()
    {
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

    public IEnumerator DashCoroutine(float dashSpeed)
{
    vulnerable = false;
    activeSpeed = dashSpeed;
    dashCounter = dashLength;

    float dashTime = 0f;
    while (dashTime < dashLength)
    {
        // Check for potential collisions
        RaycastHit2D hit = Physics2D.Raycast(transform.position, savedDirection.normalized, dashSpeed * Time.fixedDeltaTime);
        if (hit.collider != null)
        {
            // If a collision is detected, stop the dash
            break;
        }

        // Move the player in the saved direction
        rb.velocity = savedDirection.normalized * dashSpeed;

        yield return new WaitForFixedUpdate(); // Wait for the next physics update
        dashTime += Time.fixedDeltaTime;
    }

    // Reset the player's velocity after dashing
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