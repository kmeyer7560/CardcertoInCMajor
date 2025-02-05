using UnityEngine;

public class MeleeEnemyScript : MonoBehaviour
{
    public GameObject player;
    public float speed = 10f;
    public float attackRate = 2.0f;
    public float attackDuration = 2.0f;
    public float walkRange = 12f;
    public float attackRange = 1f;

    public Animator animator;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    private bool attacking;
    private Transform playerTarget;
    private float attackStartTime;
    private float nextAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        attacking = false;
        playerTarget = player.GetComponent<Transform>();
        nextAttackTime = Time.time + attackRate;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= walkRange)
        {
            if (distance <= attackRange)
            {
                if (!attacking && Time.time >= nextAttackTime)
                {
                    animator.SetTrigger("attack");
                    attacking = true;
                }
                else if (attacking && Time.time - attackStartTime >= attackDuration)
                {
                    StopAttacking();
                }
            }
            else
            {
                StopAttacking();
                attacking = false;
            }

            if (!attacking)
            {
                MoveTowardsPlayer();
                animator.SetBool("move", true);
            }
            else
            {
                animator.SetBool("move", false);
            }
        }
        else
        {
            StopAttacking();
            attacking = false;
            animator.SetBool("move", false);
        }
    }

    public void StartAttacking()
    {
        attackStartTime = Time.time;
        Attack();
    }

    void StopAttacking()
    {
        nextAttackTime = Time.time + attackRate;
    }

    void Attack()
    {
        nextAttackTime = Time.time + attackRate;
    }

    void MoveTowardsPlayer()
    {
        Vector2 directionToPlayer = (playerTarget.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);

        if (directionToPlayer.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (directionToPlayer.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}
