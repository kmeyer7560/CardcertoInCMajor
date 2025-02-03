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
    public GameObject trail;

    public Vector2 moveDirection;
    void Start()
    {
        activeSpeed = moveSpeed;
        vulnerable = true;
        moveable = true;
        trail.GetComponent<ParticleSystem>().enableEmission = false;
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

    public void dash(float dashSpeed, bool t)
    {
        Debug.Log("dashed");
        Debug.Log(dashSpeed);
        StartCoroutine(DashCoroutine(dashSpeed));
        if (t)
        {
            trail.GetComponent<ParticleSystem>().enableEmission = true;
        }
    }

    public IEnumerator DashCoroutine(float dashSpeed)
    {
        Debug.Log("Started Routine");
        vulnerable = false;
        activeSpeed = dashSpeed;
        dashCounter = dashLength;

        float dashDistance = dashSpeed * Time.fixedDeltaTime;
        trail.SetActive(true);

        int steps = Mathf.CeilToInt(dashLength / Time.fixedDeltaTime);
        Vector2 direction = savedDirection.normalized;

        for (int i = 0; i < steps; i++)
        {
            Vector2 targetPosition = rb.position + direction * dashDistance;

            RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, dashDistance);
            if (hit.collider != null && hit.collider.CompareTag("Environment"))
            {
                break;
            }

            rb.MovePosition(targetPosition);
            yield return new WaitForFixedUpdate();
        }

        // Reset the player's velocity after dashing
        StartCoroutine(DisableColliderAfterDelay(1.5f)); // Start the coroutine to disable the collider
        rb.velocity = Vector2.zero;
        activeSpeed = moveSpeed;
        dashCoolCounter = dashCooldown;
        trail.GetComponent<ParticleSystem>().enableEmission = false;
        vulnerable = true;
    }

    private IEnumerator DisableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
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
