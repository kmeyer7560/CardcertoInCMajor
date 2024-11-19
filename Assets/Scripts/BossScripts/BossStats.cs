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
    private float tractorSpeed = 5f;
    public GameObject chicken;
    public Transform chickenPos;
    public bool drive;
    Vector3 lastVelocity;
    public GameObject treadTracks;

    public GameObject player;
    private Transform playerTarget;
    Animator anim;
    public float movespeed = 10f;
    private Rigidbody2D rb;
    private bool turning;
    public Transform BossSprite;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        healthBar.SetSliderMax(maxHealth);
        player = GameObject.FindGameObjectWithTag("Player");
        playerTarget = player.GetComponent<Transform>();
    }
    public void DriveLeft()
    {
        rb.AddForce(transform.up * 700f);
        rb.AddForce(-transform.right * 500f);
        Invoke("DriveRight", 1f);
    }
    public void DriveRight()
    {
        rb.AddForce(-transform.up * 700f);
        rb.AddForce(transform.right * 500f);
        Invoke("DriveLeft", 1f);
    }
    public void TreadTracks()
    {
        Instantiate(treadTracks, BossSprite.position, Quaternion.identity);
        //temporary solution, replace with unity particles later
        Invoke("TreadTracks", 1000000000f);
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
    public void ChickenAttack()
    {
        for(int i=0;i<=10;i++)
        {
            Instantiate(chicken, chickenPos.position, Quaternion.identity);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown("p"))
        {
            ChickenAttack();
        }
        TreadTracks();
        lastVelocity = rb.velocity;
        if(drive)
        {
            DriveLeft();
        }
        if(currentHealth <= maxHealth*.5)
        {
            anim.SetTrigger("TractorPhaseTwo");
        }
        if(currentHealth <= 0)
        {
            anim.SetTrigger("TractorBossDeath");
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, coll.contacts[0].normal);

        rb.velocity = direction * Mathf.Max(speed, 0f);
    }
}
