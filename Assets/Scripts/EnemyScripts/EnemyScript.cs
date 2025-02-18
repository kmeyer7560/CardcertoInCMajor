using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public GameObject player;
    public float speed = 10f;
    public float shootRange = 6f;
    public float walkRange = 9f;

    public Animator animator;

    private bool shooting;
    private float shootStartTime;
    private float nextFireTime;

    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    public int bulletsPerShot = 1;
    public float spread = 30f;

    private NavMeshAgent agent;
    public float pathUpdateRate = 0.1f;
    private float lastPathUpdateTime;
    [SerializeField] Transform playerTransform;

    public bool isMeleeEnemy;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private float meleeCooldown = 1f;
    private float lastMeleeAttack;
    public PlayerHealthBar playerHealthBar;
    public int meleeDamage;
    private bool canAttack;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform.Find("Sprite");

        shooting = false;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.isStopped = true;

        Invoke("PlaceAgentOnNavMesh", 0.1f);
        playerHealthBar = player.GetComponentInChildren<PlayerHealthBar>();
    }

    void PlaceAgentOnNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= walkRange)
        {
            if (distance <= shootRange)
            {
                if (!shooting && Time.time >= nextFireTime)
                {
                    animator.SetTrigger("shoot");
                    shooting = true;
                    shootStartTime = Time.time;
                    if(isMeleeEnemy && Time.time - lastMeleeAttack >= meleeCooldown)
                    {
                        if(canAttack)
                        {
                            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                            foreach(Collider2D enemy in hitEnemies)
                            {
                                Debug.Log("hit" + enemy.name);
                                playerHealthBar.TakeDamage(meleeDamage);
                            }
                            canAttack = false;
                        }
                        lastMeleeAttack = Time.time;
                    }
                }
                else if (shooting && Time.time - shootStartTime >= 0)
                {
                    StopShooting();
                }
                animator.SetBool("move", false);
                agent.isStopped = true;
            }
            else
            {
                StopShooting();
                shooting = false;
                MoveTowardsPlayer();
                animator.SetBool("move", true);
            }
        }
        else
        {
            StopShooting();
            shooting = false;
            animator.SetBool("move", false);
            agent.isStopped = true;
        }

        if (Time.time - lastPathUpdateTime > pathUpdateRate)
        {
            UpdatePath();
            lastPathUpdateTime = Time.time;
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void UpdatePath()
    {
        if (player != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    void MoveTowardsPlayer()
    {
        agent.SetDestination(playerTransform.position);
        agent.isStopped = false;
        agent.speed = speed;

        // Update sprite direction
        Vector2 directionToTarget = (player.transform.position - transform.position).normalized;
        spriteRenderer.flipX = directionToTarget.x < 0;
    }

    public void StartShooting()
    {
        if(isMeleeEnemy)
        {
            canAttack = true;
        }
        else
        {
            Debug.Log("shoot");
            Shoot();
        }
    }

    void StopShooting()
    {
        nextFireTime = Time.time;
        shooting = false;
    }

    void Shoot()
    {
            Vector2 directionToPlayer = (player.transform.position - bulletPos.position).normalized;

            for (int i = 0; i < bulletsPerShot; i++)
            {
                float spreadAngle = Random.Range(-spread / 2f, spread / 2f);

                Vector2 spreadDirection = Quaternion.Euler(0, 0, spreadAngle) * directionToPlayer;

                float angle = Mathf.Atan2(spreadDirection.y, spreadDirection.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);

                GameObject bulletInstance = Instantiate(bullet, bulletPos.position, rotation);
                Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();

                rb.velocity = spreadDirection * 10f;
            }

        nextFireTime = Time.time;
    }
}
