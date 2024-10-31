using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    public BossHealthBar healthBar;
    public bool tractorBoss;
    private float tractorSpeed = 10f;

    public GameObject player;
    private Transform playerTarget;
    private Transform currentTarget;
    private bool rush;
    void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetSliderMax(maxHealth);
        player = GameObject.FindGameObjectWithTag("Player");
        playerTarget = player.GetComponent<Transform>();
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
        //if the boss is tractorboss
        if(tractorBoss)
        {
            //if current health is hald the max health then phase two
            if(currentHealth <= maxHealth * .5)
            {
                TractorPhaseTwo();
            }
            else
            {
                TractorPhaseOne();
            }
        }
        //test for takedamage
        /*if(Input.GetKeyDown("k"))
        {
            TakeDamage(100);
        }*/
    }

    public void TractorPhaseOne()
    {
        Rush();
    }

    public void Rush()
    {
        currentTarget = playerTarget;
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, tractorSpeed * Time.deltaTime);
    }
    public void TractorPhaseTwo()
    {
        Debug.Log("tractor phase two");
        //tractor explodes, chickens come out, guy shoots at you on the ground and moves like a normal enemy
    }
}
