using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class BossController : MonoBehaviour
{
    private Animator anim;
    private float maxHealth = 100;
    private float currentHealth;
    public BossHealthBar bossHealthBar;
    public PlayerHealthBar playerHealthBar;
    private int phase;
    private bool attack;
    private bool bossAlive = true;
    private bool isDead;
    public bool hittingPlayer;
    Transform playerTransform;
    CheckHit checkHit;

    public GameObject scytheBoomerang;
    string dashDirection;
    public bool attacking;
    int numOfAttacks;
    public Rigidbody2D rb;
    public GameObject player;
    public bool playerInRoom;
    private Room currentRoom;
    public Vector2 moveDirection;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerHealthBar = GameObject.Find("PlayerHealthBar").GetComponent<PlayerHealthBar>();
        bossHealthBar = GameObject.Find("BossHealthBar").GetComponent<BossHealthBar>();
        bossHealthBar.SetSliderMax(maxHealth);
        bossHealthBar.SetSlider(maxHealth);

        checkHit = GetComponentInChildren<CheckHit>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        currentHealth = maxHealth;
        phase = 0;
        rb = GetComponent<Rigidbody2D>();

        ScytheDashStart();
    }

    public void StartAttack()
    {
        phase = 1;
    }
    void Update()
    {
        UpdateHealth();
        UpdateAttack();
        UpdatePlayerInRoom();
        UpdateSpriteDirection();
    }
    void UpdateSpriteDirection()
    {
        if (spriteRenderer != null)
        {
            Vector2 directionToTarget = (player.transform.position - transform.position).normalized;
            spriteRenderer.flipX = directionToTarget.x < 0;
        }
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
    void UpdateHealth()
    {
        if(currentHealth <= maxHealth * .5f)
        {
            phase = 2;
        }
        if(currentHealth <= 0)
        {
            bossAlive = false;
            anim.SetTrigger("Death");
        }
    }
    void UpdateAttack()
    {
        if (playerInRoom)
        {
            StartAttack();
            CallAttack();
        }
    }
    void CallAttack()
    {
        if (bossAlive && !attacking)
        {
            Debug.Log("Call Atack");
            int attackType = Random.Range(0, 3);
            if (phase == 1)
            {
                if (attackType == 0)
                {
                    ScytheDashStart();
                    numOfAttacks++;
                    Debug.Log("ScytheDash");
                }
                else if (attackType == 1)
                {
                    TripleDashStart();
                    numOfAttacks++;
                    Debug.Log("TripleDash");
                }
                else if (attackType == 2)
                {
                    if (numOfAttacks > 2)
                    {
                        attacking = true;
                        StartTired();
                        Debug.Log("Tired");
                        numOfAttacks = 0;
                    }
                    else
                    {
                        CallAttack();
                    }
                }
            }
            if (phase == 2)
            {
                if (attackType == 0)
                {
                    numOfAttacks++;
                    StartScytheThrow();
                    Debug.Log("ScytheThrow");
                }
                else if (attackType == 1)
                {
                    numOfAttacks++;
                    StartBackStab();
                    Debug.Log("BackStab");
                }
                else if (attackType == 2)
                {
                    if (numOfAttacks > 2)
                    {
                        attacking = true;
                        StartTired();
                        numOfAttacks = 0;
                    }
                    else
                    {
                        CallAttack();
                    }
                }
            }
        }    
    }
    public void StartTired()
    {
        StartCoroutine(TiredSequence());
    }
    private IEnumerator TiredSequence()
    {
        anim.SetBool("Ex", true);
        yield return new WaitForSeconds(5f + (maxHealth - currentHealth) / 10);
        anim.SetBool("Ex", false);
        attacking = false;
    }
    public void DamagePlayer()
    {
        if (checkHit.isHit)
        {
            playerHealthBar.TakeDamage(10f);
        }
    }
    public void AttackEnd()
    {
        CallAttack();
    }
    public void AttackStart()
    {
        attacking = true;
    }
    
    public void ScytheDashStart()
{
    StartCoroutine(ScytheDashSequence());
}

    private IEnumerator ScytheDashSequence()
    {
        attacking = true;
        float baseForce = 10f;
        float baseDelay = 3f;
        for (int i = 1; i <= 20; i++)
        {
            int directionRange = Random.Range(1, 5);
            float currentForce = baseForce + (i / 10);
            float currentDelay = baseDelay - (i / 10);

            switch (directionRange)
            {
                case 1:
                    // Up (dash down)
                    Debug.Log("up");
                    transform.position = playerTransform.position + new Vector3(0, 5, 0);
                    anim.SetTrigger("DashDown");
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.down * currentForce, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(currentDelay - .5f);
                    rb.velocity = Vector2.zero;
                    anim.SetTrigger("SlashDown");
                    break;

                case 2:
                    // Down (dash up)
                    Debug.Log("down");
                    transform.position = playerTransform.position + new Vector3(0, -5, 0);
                    anim.SetTrigger("DashUp");
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * currentForce, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(currentDelay - .5f);
                    rb.velocity = Vector2.zero;
                    anim.SetTrigger("SlashUp");
                    break;

                case 3:
                    // Left (dash right)
                    Debug.Log("left");
                    transform.position = playerTransform.position + new Vector3(-5, 0, 0);
                    anim.SetTrigger("Dash");
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.right * currentForce, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(currentDelay - .5f);
                    rb.velocity = Vector2.zero;
                    anim.SetTrigger("Slash");
                    break;

                case 4:
                    // Right (dash left)
                    Debug.Log("right");
                    transform.position = playerTransform.position + new Vector3(5, 0, 0);
                    anim.SetTrigger("Dash");
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.left * currentForce, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(currentDelay - .5f);
                    rb.velocity = Vector2.zero;
                    anim.SetTrigger("Slash");
                    break;
            }

            yield return new WaitForSeconds(currentDelay);

        }
        Debug.Log("ScytheDashOver");
        attacking = false;
        CallAttack();
}
    public void TripleDashStart()
    {
        StartCoroutine(TripleDashSequence());
    }
    private IEnumerator TripleDashSequence()
    {
        attacking = true;
        float delay = 1f;
        float forceAmount = 2f;
        for (int i = 0; i < 3; i++)
        {
            for (int o = 0; o < 3; o++)
            {
                if (o == 2)
                {
                    forceAmount = 3f;
                    delay = 1f;
                }
                Vector2 lastPlayerPosition = (Vector2)player.transform.position;
                yield return new WaitForSeconds(delay);
                Vector2 direction = lastPlayerPosition - (Vector2)transform.position;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    // Horizontal direction
                    if (direction.x > 0)
                    {
                        // Player is right
                        anim.SetTrigger("Dash");
                        rb.velocity = Vector2.zero;
                        rb.AddForce(direction * forceAmount, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(.8f);
                        rb.velocity = Vector2.zero;
                        anim.SetTrigger("Slash");
                    }
                    else
                    {
                        // Player is left
                        anim.SetTrigger("Dash");
                        rb.velocity = Vector2.zero;
                        rb.AddForce(direction * forceAmount, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(.8f);
                        rb.velocity = Vector2.zero;
                        anim.SetTrigger("Slash");
                    }
                }
                else
                {
                    // Vertical direction
                    if (direction.y > 0)
                    {
                        // Player is up
                        anim.SetTrigger("DashUp");
                        rb.velocity = Vector2.zero;
                        rb.AddForce(direction * forceAmount, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(.8f);
                        rb.velocity = Vector2.zero;
                        anim.SetTrigger("Slash");
                    }
                    else
                    {
                        // Player is down
                        anim.SetTrigger("DashDown");
                        rb.velocity = Vector2.zero;
                        rb.AddForce(direction * forceAmount, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(.8f);
                        rb.velocity = Vector2.zero;
                        anim.SetTrigger("Slash");
                    }
                }
            }
            attacking = false;
            CallAttack();

        }
        Debug.Log("TripleDashOver");
}
    public void StartScytheThrow()
    {
    //throw boomerang scythes to the player
    anim.SetBool("toss", true);
    Instantiate(scytheBoomerang, transform.position, transform.rotation);
    }

    public void Catch()
    {
        anim.SetBool("toss", false);
        anim.SetTrigger("catch");
        StartCoroutine(CatchSequence());
    }
    private IEnumerator CatchSequence()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Catch over");
        CallAttack();

    }
    public void StartBackStab()
    {
        StartCoroutine(BackStabSequence());
    }

    private IEnumerator BackStabSequence()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        if (playerMovement.savedDirection.x < 0)
        {
            // Player is facing LEFT
            gameObject.SetActive(true);
            transform.position = new Vector3(player.transform.position.x + 1, player.transform.position.y);
            anim.SetTrigger("Slash");
            yield return new WaitForSeconds(.8f);
            Debug.Log("backstab over");
            CallAttack();
        }

        if (playerMovement.savedDirection.x > 0)
        {
            // Player is facing RIGHT
            gameObject.SetActive(true);
            transform.position = new Vector3(player.transform.position.x - 1, player.transform.position.y);
            anim.SetTrigger("Slash");
            yield return new WaitForSeconds(.8f);
            Debug.Log("backstab over");
            CallAttack();
        }
    

    }
}
