using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class EnemyHealth : MonoBehaviour
{
    public GameObject player;
    public float health;
    public float currentHealth;
    private Vector2 source;
    public Rigidbody2D rb;
    public int violinStacks;

    public ParticleSystem pc;

    public GameObject healthOrb;
    public GameObject coin;
    private Vector2 vel;

    public Animator anim;
    public bool isAlive;
    private SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = health;
        isAlive = true;
        renderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 && isAlive)
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
            Instantiate(healthOrb, transform.position, Quaternion.identity);
        }
        if(dropChance == 2)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }
        anim.SetTrigger("dead");
        isAlive = false;

        StartCoroutine(FadeToDeath());
    }

    private IEnumerator FadeToDeath()
    {
        yield return new WaitForSeconds(1f);
        Color originalColor = renderer.color;

        for(float i=0; i<2f; i += Time.deltaTime)
        {
            Debug.Log("dying");
            float normalizedTime = i/2f;
            float alpha = Mathf.Lerp(1f, 0f, normalizedTime);
            renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        Destroy(gameObject);
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(HitFlash());
    }

    IEnumerator HitFlash()
    {
        renderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        renderer.color = Color.white;
    }

    public void detonate()
    {
        currentHealth -= (violinStacks * 5);    
        violinStacks = 0;
        pc.GetComponent<violinStacks>().SetMaxParticles(violinStacks);
    }

    public void addStack(int i)
    {
        pc.GetComponent<violinStacks>().SetMaxParticles(violinStacks);
    }

    public void deflectSlash()
    {
        violinStacks += 2;
        takeDamage(1);
        pc.GetComponent<violinStacks>().SetMaxParticles(violinStacks);
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
        yield return new WaitForSeconds(0.1f);
        rb.velocity = vel;
    }

    void OnParticleCollision(GameObject particle)
    {
        takeDamage(1);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fDashCard"))
        {
            takeDamage(15);
        }
    }
}
