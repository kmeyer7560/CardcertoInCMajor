using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public GameObject player;
    public float fireRate = 2.0f;
    public float speed = 10f;
    public float shootDuration = 2f;
    public float range = 10f;

    public Animator animator;

    public float spreadAmount;

    private bool shooting;
    private Transform playerTarget;
    private float shootStartTime;
    private float lastShotTime;

    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shooting = false;
        playerTarget = player.GetComponent<Transform>();
        lastShotTime = -fireRate;
    }

    void Update()
    {
        // Update distance from player
        float distance = Vector2.Distance(transform.position, player.transform.position);

        // If distance is less than set range
        if (distance <= range)
        {
            if (!shooting)
            {
                if ((int)(Time.time % fireRate) == 0)
                {
                    animator.SetTrigger("shoot");
                    StartShooting();
                }
            }
            else
            {
                if (Time.time - shootStartTime >= shootDuration)
                {
                    StopShooting();
                }
            }
        }
        else
        {
            StopShooting();
        }

        // If not shooting then move towards player
        if (!shooting)
        {
            MoveTowardsPlayer();
            animator.SetBool("move", true);
        }
        else
        {
            animator.SetBool("move", false);
        }
    }

    void StartShooting()
    {
        shooting = true;
        shootStartTime = Time.time;
        Shoot();
    }

    void StopShooting()
    {
        shooting = false;
    }

    void Shoot()
    {
        if (Time.time - lastShotTime >= fireRate)
        {
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
            lastShotTime = Time.time;
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate direction to player
        Vector2 directionToPlayer = (playerTarget.position - transform.position).normalized;

        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);

        // Flip the sprite based on the direction
        if (directionToPlayer.x < 0)
        {
            spriteRenderer.flipX = true; // Facing left
        }
        else if (directionToPlayer.x > 0)
        {
            spriteRenderer.flipX = false; // Facing right
        }
    }
}
