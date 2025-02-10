using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject player;
    public float health;
    public float currentHealth;
    private Vector2 source;
    public Rigidbody2D rb;
    public int violinStacks;

    public GameObject healthOrb;
    public GameObject coin;

    private Vector2 vel;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 )
        {
            Death();
        }
    }

    public void Death()
    {
        int dropChance = Random.Range(1,10);
        Debug.Log(dropChance);
        if(dropChance <=5)
        {
            Instantiate(healthOrb, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
        }
        if(dropChance == 2)
        {
            Instantiate(coin, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
        }
        Destroy(gameObject);

    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void detonate()
    {
        currentHealth -= (violinStacks * 5);
        violinStacks = 0;
    }

    public void knockBack(GameObject attack)
    {
        vel = rb.velocity;
        source = rb.transform.position - attack.transform.position;
        StartCoroutine(knockBackRoutine());

    }

    IEnumerator knockBackRoutine()
    {
        rb.velocity = (source * 13f);
        yield return new WaitForSeconds(0.2f);
        rb.velocity = vel;
    }

    void OnParticleCollision(GameObject particle)
    {
        takeDamage(1);
    }
}
