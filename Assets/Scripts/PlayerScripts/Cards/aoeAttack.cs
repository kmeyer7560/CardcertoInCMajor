using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class aoeAttack : MonoBehaviour
{

    public GameObject Player;
    Rigidbody2D rb;
    public float damage;
    public bool kb;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(attackRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator attackRoutine()
    {   
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().takeDamage(damage);
            if (kb)
            {
                other.GetComponent<EnemyHealth>().knockBack(this.gameObject);
            }
        }
    }
}
