using UnityEngine;

public class MeleeEnemyScript : MonoBehaviour
{
    public GameObject player;
    public float speed = 10f;

    private float timer;
    private bool attacking;
    private Transform playerTarget;
    private float attackStartTime;
    public int range = 3;
    public float attackRate = 2.0f;
    public float attackDuration = 2.0f;
    void Start()
    {
        //player is the object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        //automatically makes shooting false so they dont shoot before being in range
        attacking = false;
        //sets the player's transform component necessary for knowing where the player is
        playerTarget = player.GetComponent<Transform>();
    }

    void Update()
    {
        //updates distance from player
        float distance = Vector2.Distance(transform.position, player.transform.position);
        
        //if distance is less then set range
        if (distance <= range)
        {
            //if not shooting
            if (!attacking)
            {
                //add 1 second to shoot timer
                timer += Time.deltaTime;
                
                //firerate
                if (timer >= attackRate)
                {
                    StartAttacking();
                }
            }
            //else if shooting
            else
            {
                //calculates shoot duration. Current time since the start of the game - when the enemy starts shooting >= set shoot duration variable. EX: if the shoot duration is 2 then stop after two seconds until told to start shooting again.
                if (Time.time - attackStartTime >= attackDuration)
                {
                    StopAttacking();
                }
            }
        }
        //else if enemy is out of range
        else
        {
            StopAttacking();
        }
        
        //if not shooting then move towards player
        if (!attacking)
        {
            MoveTowardsPlayer();
        }
    }

    void StartAttacking()
    {
        attacking = true;
        //shootStartTime = current time since started the game
        attackStartTime = Time.time;
        Attack();
    }

    void StopAttacking()
    {
        attacking = false;
        // timer resets for shooting
        timer = 0;
    }

    void Attack()
    {
        //deak damage to player and do animation
    }

    void MoveTowardsPlayer()
    {
        //calculate where the  player is
        Vector2 directionToPlayer = (playerTarget.position - transform.position).normalized;
        //move towards the player
        transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);
    }
}