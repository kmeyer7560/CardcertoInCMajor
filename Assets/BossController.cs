using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private Animator anim;
    private float maxHealth;
    private float currentHealth;
    public BossHealthBar healthBar;
    public PlayerHealthBar playerHealthBar;
    private bool phase1;
    private bool phase2;
    private bool attack;

    private bool bossAlive = true;
    public bool hittingPlayer;
    Transform playerTransform;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("PhaseOne", true);

        currentHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        playerHealthBar = GetComponent<PlayerHealthBar>();
        hittingPlayer = GetComponent<checkHit>().isHit;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    public void StartAttack()
    {
        phase1 = true;
    }
    void Update()
    {
        UpdateHealth();
    }
    void UpdateHealth()
    {
        if(currentHealth <= maxHealth * .5f)
        {
            anim.SetTrigger("Transition");
            anim.SetBool("PhaseOne", false);
            anim.SetBool("PhaseTwo", false);
        }
        if(currentHealth <= 0)
        {
            bossAlive = false;
            anim.SetTrigger("Death");
        }
    }
    void CallAttack()
    {
        if(bossAlive)
        {
            int atkType = Random.Range(1,5);
            switch(atkType)
            {
                case 1:
                anim.SetTrigger("Atk1");
                break;

                case 2:
                anim.SetTrigger("Atk2");
                break;

                case 3:
                anim.SetTrigger("Atk3");
                break;

                case 4:
                anim.SetTrigger("Ex");
                break;
            }
        }
    }
    //Attack End
    public void AttackEnd()
    {
        CallAttack();
    }
    //PHASE ONE
    public void ScytheSwipeStart()
    {
        //teleport ot random position
        float horizontalRange = Random.Range(-10, 10);
        float verticalRange = Random.Range(-10, 10);
        transform.position = new Vector3(horizontalRange * Time.deltaTime, verticalRange * Time.deltaTime);
    }
    public void ScytheSwipeAttack()
    {
        //if the player is in attack area then deal damage
        if(hittingPlayer)
        {
            playerHealthBar.TakeDamage(10);
        }
        
    }
    public void ScytheDashStart()
    {
        //teleports to top, left, right, or bottom of player
        int directionRange = Random.Range(1,5);
        switch(directionRange)
        {
            case 1:
            //top
            //transform.position = new Vector3(playerTransform);
            break;

            case 2:
            //bottom
            break;

            case 3:
            //left
            break;

            case 4:
            //right
            break;
        }
    }
    public void ScytheDashAttack()
    {
        //charges towards the player
    }
    public void ScytheSpinStart()
    {
        //teleports to the center of the map
    }
    public void ScytheSpinAttack()
    {
        //send slash attack clones to the player
    }
    //PHASE TWO
    public void ScytheThrow()
    {
        //throw boomerang scythes to the player
    }
    public void ScytheSpeedStart()
    {
        //teleport around the edge of the map
    }
    public void ScytheSpeedAttack()
    {
        //charge towards the player
    }
    public void Tornado()
    {
        //bounce around the map randomly
    }
    //DEATH
    public void BossDeath()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"));
        {
            playerHealthBar.TakeDamage(20f);
        }
    }
}
