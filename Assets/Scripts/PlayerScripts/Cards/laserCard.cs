using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class laserCard : MonoBehaviour
{
    public GameObject Player;
    private Rigidbody2D rb;
    public float damage;

    private SpriteRenderer sr;
    private PlayerMovement playerMovement; // Reference to PlayerMovement script
    public float distanceFromPlayer = 1.0f; // Distance to place the laserCard in front of the player

    // Start is called before the first frame update
    void Start()
    {   
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerMovement = Player.GetComponent<PlayerMovement>(); // Get the PlayerMovement component
        StartCoroutine(attackRoutine());
        
        // Rotate and position the laserCard based on the player's savedDirection
        RotateAndPositionLaserCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RotateAndPositionLaserCard()
    {
        // Assuming savedDirection is a Vector2 in PlayerMovement
        Vector2 savedDirection = playerMovement.savedDirection; // Access the savedDirection
        float angle = Mathf.Atan2(savedDirection.y, savedDirection.x) * Mathf.Rad2Deg; // Calculate angle in degrees
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Apply rotation

        // Position the laserCard in front of the player
        Vector3 playerPosition = Player.transform.position; // Get the player's position
        Vector3 offset = savedDirection.normalized * distanceFromPlayer; // Calculate the offset
        transform.position = playerPosition + offset; // Set the laserCard's position
    }

    IEnumerator attackRoutine()
    {   
        yield return new WaitForSeconds(1f);
        Player.GetComponent<PlayerMovement>().moveable = true;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().takeDamage(damage);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().takeDamage(damage);
            StartCoroutine(laserCooldown());
            
        }
    }

    IEnumerator laserCooldown()
    {
        yield return new WaitForSeconds(0.3f);
    }
}
