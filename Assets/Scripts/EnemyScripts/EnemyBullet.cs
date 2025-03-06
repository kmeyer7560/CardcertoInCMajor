using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force = 10f;
    private float timer;
    public float damage;
    public GameObject hitMarker;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            PlayerHealthBar healthBar = GameObject.Find("PlayerHealthBar").GetComponent<PlayerHealthBar>();

            if (playerMovement != null && playerMovement.vulnerable)
            {
                healthBar.TakeDamage(damage);
            }
        }

        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
            Instantiate(hitMarker, hitPoint, Quaternion.identity);
        }
    }
}
