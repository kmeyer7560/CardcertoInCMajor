using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force = 10f;
    private float timer;
    public float damage;
    public GameObject hitMarker;
    PlayerMovement playerMovement; 
    PlayerHealthBar healthBar;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<PlayerHealthBar>();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 10)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 hitPoint = other.ClosestPoint(transform.position);

        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Instantiate(hitMarker, hitPoint, Quaternion.identity);
            if (playerMovement.vulnerable)
            {
                healthBar.TakeDamage(damage);
            }
        }

        if (other.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
            Instantiate(hitMarker, hitPoint, Quaternion.identity);
        }
    }
}
