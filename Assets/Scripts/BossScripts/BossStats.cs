using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    public BossHealthBar healthBar;
    public bool tractorBoss;
    private float tractorSpeed = 5f;

    public GameObject player;
    private Transform playerTarget;
    Animator anim;
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        healthBar.SetSliderMax(maxHealth);
        player = GameObject.FindGameObjectWithTag("Player");
        playerTarget = player.GetComponent<Transform>();
    }

    public void Attack(float amount)
    {
        //attack player
    }
    public void TakeDamage(float amount)
    {
        //subtracts the health by the amount inputed when function is called and updates slider
        currentHealth -= amount;
        healthBar.SetSlider (currentHealth);
    }
    public void Heal(float amount)
    {
        //adds the health by the amount inputed when function is called and updates slider
        currentHealth += amount;
        healthBar.SetSlider(currentHealth);
    }
    private void Update()
    {
        if(currentHealth <= maxHealth*.5)
        {
            anim.SetTrigger("TractorPhaseTwo");
        }
        if(currentHealth <= 0)
        {
            anim.SetTrigger("TractorBossDeath");
        }
    }
}
