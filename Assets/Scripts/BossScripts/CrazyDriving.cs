using UnityEngine;

public class CrazyDriving : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float reverseSpeed = 3f;
    public float rotationSpeed = 100f;
    public float visionRange = 10f;
    public float ramSpeed = 15f;
    public float reverseTime = 2f;
    public LayerMask playerLayer;
    public LayerMask wallLayer;

    private Rigidbody2D rb;
    private Transform player;
    private bool isRamming = false;
    private bool isReversing = false;
    private Vector2 lastKnownPlayerPosition;
    private Vector2 ramDirection;
    private float reverseTimer;
    private bool rotateClockwise;

    public bool drive;

    private float currentRamSpeed = 10f;
    private float accelerationRate = 30f;
    private float maxRamSpeed = 100f;

    void Start()
    {
        drive = false;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if(drive)
        {
            if (isReversing)
            {
                ReverseFromWall();
            }
            else if (isRamming)
            {
                RamTowardsPlayer();
            }
            else if (CanSeePlayer())
            {
                PrepareToRam();
            }
            else
            {
                SearchForPlayer();
            }
        }
    }

    bool CanSeePlayer()
    {
        Debug.Log("Boss is checking for player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, visionRange, playerLayer);
        if (hit.collider != null)
        {
            Debug.Log("Boss has spotted player");
            lastKnownPlayerPosition = hit.point;
            return true;
        }
        return false;
    }

    void PrepareToRam()
    {
         if(drive)
        {
            Debug.Log("Boss is preparing to ram");
            Vector2 directionToPlayer = (lastKnownPlayerPosition - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);

            if (Mathf.Abs(Mathf.DeltaAngle(rb.rotation, angle)) < 5f)
            {
                isRamming = true;
                ramDirection = directionToPlayer;
                currentRamSpeed = 20f; // Reset the speed when starting a new ram
            }
        }
    }

    void RamTowardsPlayer()
    {
        Debug.Log("Boss is ramming");
    
    // Gradually increase the speed
    currentRamSpeed = Mathf.Min(currentRamSpeed + accelerationRate * Time.deltaTime, maxRamSpeed);
    rb.velocity = ramDirection * currentRamSpeed;
    }

    void ReverseFromWall()
    {
        Debug.Log("Boss is reversing");
        rb.velocity = -transform.up * reverseSpeed;
        reverseTimer -= Time.fixedDeltaTime;
        if (reverseTimer <= 0)
        {
            isReversing = false;
            isRamming = false;
        }
    }

    void SearchForPlayer()
    {
        if(drive)
        {
            Debug.Log("Boss is searching");
            rb.velocity = transform.up * moveSpeed;
            float rotationDirection = rotateClockwise ? -1 : 1;
            rb.angularVelocity = rotationSpeed * rotationDirection;
            if (Random.value < 0.02f)
            {
                rotateClockwise = !rotateClockwise;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ArenaWall"))
        {
            Debug.Log("Boss hit a wall");
            isRamming = false;
            isReversing = true;
            reverseTimer = reverseTime;
        }
    }
}
