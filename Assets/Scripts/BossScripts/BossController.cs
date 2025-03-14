using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private Animator anim;
    private float maxHealth = 100;
    private float currentHealth;
    public BossHealthBar bossHealthBar;
    public PlayerHealthBar playerHealthBar;
    private bool phase1;
    private bool phase2;
    private bool attack;

    private bool bossAlive = true;
    public bool hittingPlayer;
    Transform playerTransform;
    CheckHit checkHit;
    Transform lastPlayerPosition;

    public GameObject scytheProjectile;
    public GameObject scytheBoomerang;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerHealthBar = GameObject.Find("PlayerHealthBar").GetComponent<PlayerHealthBar>();
        bossHealthBar = GameObject.Find("BossHealthBar").GetComponent<BossHealthBar>();
        bossHealthBar.SetSliderMax(maxHealth);
        bossHealthBar.SetSlider(maxHealth);
        checkHit = GetComponentInChildren<CheckHit>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        currentHealth = maxHealth;
        anim.SetBool("PhaseOne", true);
    }

    public void StartAttack()
    {
        phase1 = true;
    }
    void Update()
    {
        UpdateHealth();
        UpdateAttack();
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
    void UpdateAttack()
    {
        if(checkHit.isHit)
        {
            hittingPlayer = true;
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
    public void ScytheSwipe()
    {
        //teleports to random pos
        float horizontalRange = Random.Range(-20f,21f);
        float verticalRange = Random.Range(-20f,21f);
        transform.position = playerTransform.position + new Vector3(horizontalRange, verticalRange, 0);
    }
    public void ScytheSwipeAttack()
    {
        //hits player if in range
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
            //up
            transform.position = playerTransform.position + new Vector3(0,20,0);
            break;

            case 2:
            //down
            transform.position = playerTransform.position + new Vector3(0,-20,0);
            break;

            case 3:
            //left
            transform.position = playerTransform.position + new Vector3(-20,0,0);
            break;

            case 4:
            //right
            transform.position = playerTransform.position + new Vector3(20,0,0);
            break;

        }
    }
    public void ScytheDashAttack()
    {
        //charges towards the player
        playerTransform = lastPlayerPosition;
        transform.position = Vector3.MoveTowards(transform.position, lastPlayerPosition.position, 3);
    }
    public void ScytheSpinStart()
    {
        //teleports to the center of the map
        transform.position = new Vector3(0, 0, 0);
    }
    public void ScytheSpinAttack()
    {
        //send slash attack clones to the player
        Instantiate(scytheProjectile, transform.position, transform.rotation);
    }
    //PHASE TWO
    public void ScytheThrow()
    {
        //throw boomerang scythes to the player
        Instantiate(scytheBoomerang, transform.position, transform.rotation);
    }
    public void ScytheSpeedStart()
    {
        //teleport around the edge of the map
    }
    public void ScytheSpeedAttack()
    {
        //charge towards the player
        playerTransform = lastPlayerPosition;
        transform.position = Vector3.MoveTowards(transform.position, lastPlayerPosition.position, 6);
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
}
