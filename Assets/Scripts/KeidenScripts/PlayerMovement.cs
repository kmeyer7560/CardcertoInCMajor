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
     

    public Rigidbody2D rb;

    public Vector2 moveDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        activeSpeed = moveSpeed;
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

    public void Dash(float dashSpeed)
    {
    
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                activeSpeed = dashSpeed;
                dashCounter = dashLength;
            }
    }

    // Update is called once per frame
    void Update() 
    {
        ProcessInputs();
        if(savedDirection!= moveDirection && moveDirection!= Vector2.zero){ //ian coded
            savedDirection = moveDirection; //ian coded
        }
        }
}