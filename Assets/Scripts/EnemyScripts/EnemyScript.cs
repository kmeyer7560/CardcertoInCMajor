using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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
    public bool isDoubleEnemy;
    public Transform attackPoint;
    public GameObject grapplePoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private float meleeCooldown = 1f;
    private float lastMeleeAttack;
    private float lastDoubleAttack;
    public PlayerHealthBar playerHealthBar;
    public int meleeDamage;
    private bool canAttack = true;
    private bool cooldown = false;
    private bool playerInRoom;
    private Room currentRoom;
    public GameObject shootFX;
    public bool canMove;

    ParticleSystem shootFXParticleSystem;
    public Quaternion shootRotation;

    private bool isShootingAnimation = false;
    public bool canLunge;

     private bool isCharging = false;
     public float chargeRange = 4f;

    void Start()
    {
        canMove = true;
        if(shootFX != null)
        {
            shootFXParticleSystem = shootFX.GetComponentInChildren<ParticleSystem>();
        }
        playerHealthBar = GameObject.Find("PlayerHealthBar").GetComponent<PlayerHealthBar>();

        grapplePoint = GameObject.FindGameObjectWithTag("grapplePoint");
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform.Find("Sprite");
        }
        
        shooting = false;

        currentRoom = GetComponentInParent<Room>();
    }

    void Update()
    {
        if(animator != null)
        {
        }
        if(isDoubleEnemy)
        {
            DoubleEnemyUpdate();
        }
        if (player == null || playerTransform == null || currentRoom == null)
        {
            return;
        }

        UpdatePlayerInRoom();
        UpdateSpriteDirection();

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (playerInRoom && GetComponentInChildren<EnemyHealth>().isAlive)
        {
            if (distance <= walkRange)
            {
                if (distance <= shootRange)
                {
                    if(isDoubleEnemy) 
                    {
                        StartShootingSequence();
                    }
                    else if (!shooting && Time.time >= nextFireTime)  
                    {
                        StartShootingSequence();
                    }
                    else if (shooting && Time.time - shootStartTime >= 0)
                    {
                        StopShooting();
                    }
                    animator.SetBool("move", false);
                    canMove = false;
                }
                else
                {
                    StopShooting();
                    shooting = false;
                    canMove = !isShootingAnimation;
                    if (canMove)
                    {
                        MoveTowardsPlayer();
                        animator.SetBool("move", true);
                    }
                }
            }
            else
            {
                StopShooting();
                shooting = false;
                animator.SetBool("move", false);
                canMove = false;
            }
        }
        else
        {
            StopShooting();
            shooting = false;
            animator.SetBool("move", false);
            canMove = false;
        }

        if (Time.time - lastPathUpdateTime > pathUpdateRate)
        {
            lastPathUpdateTime = Time.time;
        }
    }
    public void getHooked()
    {
        Debug.Log("hook started");
        Vector2 target = grapplePoint.transform.position;
        this.GetComponent<EnemyHealth>().takeDamage(25);
        canMove = false; // Disable movement
        canAttack = false; // Disable attack
        StartCoroutine(hookRoutine(target));
    }

    IEnumerator hookRoutine(Vector2 target)
    {
        float duration = 0.3f; // Duration of the pull
        float elapsedTime = 0f;

        Vector2 startingPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startingPosition, target, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the enemy is exactly at the target position after the loop
        transform.position = target;

        canAttack = true; // Re-enable attack
        canMove = true; // Re-enable movement
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
        animator.SetBool("move", true);
        if (player == null || playerTransform == null || isShootingAnimation)
        {
            return;
        }

        if(playerInRoom && canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, Time.deltaTime * speed);
        }
    }

   void DoubleEnemyUpdate()
    {
        if(isCharging && isDoubleEnemy)
        {
            animator.SetBool("move", true);
        }
    }

    void StartShootingSequence()
    {
        canMove = false;
        animator.SetTrigger("shoot");
        shooting = true;
        shootStartTime = Time.time;

        if (isMeleeEnemy && Time.time - lastMeleeAttack >= meleeCooldown)
        {
            if (canAttack)
            {
                PerformMeleeAttack();
            }
            lastMeleeAttack = Time.time;
        }
        if(isDoubleEnemy && !cooldown && !isCharging)
        {
            DaggerAttack();
            StartCoroutine(DaggerCooldown());
        }
        
    }

        void DaggerAttack()
    {
        int switchAttack = Random.Range(0,3);
        {

            if(switchAttack == 0 )
            {
                //melee
                DaggerCharge();
            }
            else if(switchAttack == 1 || switchAttack == 2)
            {
                //ranged
                animator.SetTrigger("throw");
            }
        }
    }


    private IEnumerator DaggerCooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(1f);
        cooldown = false;

    }

    void PerformMeleeAttack()
    {
        //Debug.Log("Performing Melee Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            //Debug.Log("Hit: " + enemy.name);
            playerHealthBar.TakeDamage(meleeDamage);
        }
        canAttack = false;
    }


    public void Lunge()
    {
        int lungeSpeed = 3;
        if(isDoubleEnemy){lungeSpeed = 2;}
        rb.velocity = (playerTransform.position - transform.position).normalized * lungeSpeed;
        StartCoroutine(LungeDuration());
    }

    private IEnumerator LungeDuration()
    {
        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector2.zero;

    }

    void UpdatePlayerInRoom()
    {
        CameraController cameraController = CameraController.instance;
        if (cameraController != null)
        {
            playerInRoom = cameraController.currRoom == currentRoom;
        }
        else
        {
            playerInRoom = false;
        }
    }

    void UpdateSpriteDirection()
    {
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
        canAttack = false;
    }

    void Shoot()
    {
        if (player == null || bulletPos == null)
        {
            return;
        }

        Vector2 directionToPlayer = (player.transform.position - bulletPos.position).normalized;
        Vector2 shootDirection = Quaternion.Euler(0, 0, 0) * directionToPlayer;
        float shootAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        shootRotation = Quaternion.Euler(-shootAngle, 90, 90);
        if(shootFX != null)
        {
            shootFX.transform.rotation = shootRotation;
            shootFXParticleSystem.Play();
            //Debug.Log("ShootFX is played");
        }

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

            if (bulletRb != null)
            {
                bulletRb.velocity = spreadDirection * 10f;
            }
        }

        nextFireTime = Time.time;
    }

    // Animation event methods
    public void OnShootAnimationStart()
    {
        isShootingAnimation = true;
        canMove = false;
    }

    public void OnShootAnimationEnd()
    {
        isShootingAnimation = false;
        canMove = true;
    }

    public void DaggerStab()
    {
        PerformMeleeAttack();
    }

      void DaggerCharge()
    {
        if (isCharging) return;

        StartCoroutine(ChargeTowardsPlayer());
    }

    IEnumerator ChargeTowardsPlayer()
{
    isCharging = true;
    canMove = true;
    Debug.Log("charging");

    while (Vector2.Distance(transform.position, playerTransform.position) > 2f && isCharging)
    {
        animator.SetBool("move", true);
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * 15 * Time.deltaTime);
        yield return null;
    }

    if (isCharging)
    {
        animator.SetTrigger("stab");
        isCharging = false;
        canMove = false;
    }
}


}
