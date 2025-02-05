using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public float health;
    public float currentHealth;
    private Vector2 source;
    public Rigidbody2D rb;
    public int violinStacks;

    private Vector2 vel;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 )
        {
            Destroy(gameObject);
        }
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
