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
    private bool playerInRoom;
    private Room currentRoom;

    ParticleSystem particleSystem;

    void Start()
    {
        playerHealthBar = GameObject.Find("PlayerHealthBar").GetComponent<PlayerHealthBar>();

        if(!isMeleeEnemy)
        {
            attackPoint = null;
        }
        particleSystem = GetComponentInChildren<ParticleSystem>();

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform.Find("Sprite");
        }
        
        shooting = false;

        if (player != null)
        {
        }

        currentRoom = GetComponentInParent<Room>();

        // Check for other necessary components
    }

    void Update()
    {
        if (player == null || playerTransform == null)
        {
            return;
        }

        if (currentRoom == null)
        {
            return;
        }

        CameraController cameraController = CameraController.instance;
        if (cameraController != null)
        {
            if (cameraController.currRoom == currentRoom)
            {
                playerInRoom = true;
            }
            else
            {
                playerInRoom = false;
            }
        }
        else
        {
            playerInRoom = false;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if(playerInRoom)
        {
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
            }
        }
        if (Time.time - lastPathUpdateTime > pathUpdateRate)
        {
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

        void MoveTowardsPlayer()
    {
        //agent.SetDestination(playerTransform.position);
        //agent.isStopped = false;
        //agent.speed = speed;
        if (player == null || playerTransform == null)
        {
            return;
        }

        if(playerInRoom)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, Time.deltaTime * speed);
        }
        // Update sprite direction
        if (spriteRenderer != null)
        {
            Vector2 directionToTarget = (player.transform.position - transform.position).normalized;
            spriteRenderer.flipX = directionToTarget.x < 0;
        }
    }


    public void StartShooting()
    {
        if(isMeleeEnemy)
        {
            canAttack = true;
        }
        else
        {
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
        if (particleSystem == null)
        {
            return;
        }
        particleSystem.Play();

        if (player == null || bulletPos == null)
        {
            return;
        }

        Vector2 directionToPlayer = (player.transform.position - bulletPos.position).normalized;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            float spreadAngle = Random.Range(-spread / 2f, spread / 2f);

            Vector2 spreadDirection = Quaternion.Euler(0, 0, spreadAngle) * directionToPlayer;

            float angle = Mathf.Atan2(spreadDirection.y, spreadDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            if (bullet == null)
            {
                return;
            }

            GameObject bulletInstance = Instantiate(bullet, bulletPos.position, rotation);
            Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();

            if (bulletRb == null)
            {
                
            }
            else
            {
                bulletRb.velocity = spreadDirection * 10f;
            }
        }

        nextFireTime = Time.time;
    }
}
