using UnityEngine;

public class MeleeEnemyScript : MonoBehaviour
{
    public GameObject player;
    public float speed = 10f;

    private bool attacking;
    private Transform playerTarget;
    private float attackStartTime;
    public int range = 3;
    public float attackRate = 2.0f;
    public float attackDuration = 2.0f;
    private float lastAttackTime;

    void Start()
    {
        // player is the object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        // automatically makes shooting false so they don't shoot before being in range
        attacking = false;
        // sets the player's transform component necessary for knowing where the player is
        playerTarget = player.GetComponent<Transform>();
        lastAttackTime = -attackRate; // Initialize to allow immediate first attack
    }

    void Update()
    {
        // updates distance from player
        float distance = Vector2.Distance(transform.position, player.transform.position);
        
        // if distance is less than set range
        if (distance <= range)
        {
            // if not attacking
            if (!attacking)
            {
                // Check if enough time has passed since the last attack
                if (Time.time - lastAttackTime >= attackRate)
                {
                    StartAttacking();
                }
            }
            // else if attacking
            else
            {
                // calculates attack duration. Current time since the start of the game - when the enemy starts shooting >= set attack duration variable.
                if (Time.time - attackStartTime >= attackDuration)
                {
                    StopAttacking();
                }
            }
        }
        // else if enemy is out of range
        else
        {
            StopAttacking();
        }
        
        // if not attacking then move towards player
        if (!attacking)
        {
            MoveTowardsPlayer();
        }
    }

    void StartAttacking()
    {
        attacking = true;
        // attackStartTime = current time since started the game
        attackStartTime = Time.time;
        Attack();
    }

    void StopAttacking()
    {
        attacking = false;
    }

    void Attack()
    {
        lastAttackTime = Time.time; // Update last attack time
        // deal damage to player and do animation
        Debug.Log("attack player");
    }

    void MoveTowardsPlayer()
    {
        // calculate where the player is
        Vector2 directionToPlayer = (playerTarget.position - transform.position).normalized;
        // move towards the player
        transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);
    }
}