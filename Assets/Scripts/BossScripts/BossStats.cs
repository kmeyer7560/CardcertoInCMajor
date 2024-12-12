using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    public BossHealthBar healthBar;
    public bool tractorBoss;
    public GameObject chicken;
    public Transform chickenPos;

    public GameObject player;
    private Transform playerTarget;
    Animator anim;

    private int chickens = 10;

    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetSliderMax(maxHealth);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            TakeDamage(100);
        }
        if (currentHealth <= maxHealth * 0.5f)
        {
            if (anim != null)
            {
                anim.SetTrigger("TractorPhaseTwo");
            }
        }
        if (currentHealth <= 0)
        {
            if (anim != null)
            {
                anim.SetTrigger("TractorBossDeath");
            }
        }
    }

    public void Attack(float amount)
    {
        // Attack player logic here
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (healthBar != null)
        {
            healthBar.SetSlider(currentHealth);
        }
    }
    
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (healthBar != null)
        {
            healthBar.SetSlider(currentHealth);
        }
    }

    public void ChickenAttack()
    {
        if (chickens > 0 && chicken != null && chickenPos != null)
        {
            Instantiate(chicken, chickenPos.position, Quaternion.identity);
            chickens--;
            Invoke("ChickenAttack", 0.1f);
        }
    }
}
