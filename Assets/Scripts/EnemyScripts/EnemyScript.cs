using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public GameObject player;
    public float fireRate = 2.0f;
    public float speed = 10f;
    public float shootDuration = 2f;
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

    // NavMesh variables
    private NavMeshAgent agent;
    public float pathUpdateRate = 0.1f;
    private float lastPathUpdateTime;
    [SerializeField] Transform playerTransform;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //playerTransform = GameObject.FindGameObjectWithTag("PlayerSprite").transform;

        shooting = false;
        nextFireTime = Time.time + fireRate;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        PlaceAgentOnNavMesh();
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
                }
                else if (shooting && Time.time - shootStartTime >= shootDuration)
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
        Debug.Log("shoot");
        Shoot();
    }

    void StopShooting()
    {
        nextFireTime = Time.time + fireRate;
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

        nextFireTime = Time.time + fireRate;
    }
}
