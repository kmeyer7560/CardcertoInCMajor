using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dumAOE : MonoBehaviour
{
    public GameObject playerHealth;
    Rigidbody2D rb;
    public float damage;
    int hitEnemies;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerHealth = GameObject.FindGameObjectWithTag("playerHealthBar");
        hitEnemies = 0;
        StartCoroutine(attackRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator attackRoutine()
    {   
        yield return new WaitForSeconds(0.3f);
        playerHealth.GetComponent<PlayerHealthBar>().Heal(10, hitEnemies);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().takeDamage(damage);
            hitEnemies++;
            
        }
    }
}
