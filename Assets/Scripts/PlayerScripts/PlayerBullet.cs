using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBullet : MonoBehaviour
{
    public float Speed;

    public float damage;

    public GameObject Player;

    Rigidbody2D rb;

    private float timer;

    public Transform objectInRay;

    private GameObject collidedObject;

    // Start is called before the first frame update
    void Start()
{
    Player = GameObject.FindGameObjectWithTag("Player");
    rb = GetComponent<Rigidbody2D>();

    // Get the FOV component from the player
    FOV fov = Player.GetComponent<FOV>();
    objectInRay = fov.hitObject;

    if (objectInRay != null) // If there is an object detected by the rays
    {
        // Calculate the direction to the hit object
        Vector2 directionToHitObject = (objectInRay.position - transform.position).normalized;

        // Set the velocity towards the hit object
        rb.velocity = directionToHitObject * Speed;
    }
    else // If no object is detected, use the saved direction
    {
        rb.velocity = Player.GetComponent<PlayerMovement>().savedDirection * Speed;
    }
}


    // Update is called once per frame
    void Update()
    {    
        if (objectInRay != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, objectInRay.position, Speed * Time.deltaTime);
        }

        Speed += 0.1f;

        timer += Time.deltaTime;
        // Bullet lasts 10 seconds before it dies
        if (timer > 6)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (this.tag == "violinAttack")
            {
                other.GetComponent<EnemyHealth>().violinStacks += 1;
                other.GetComponent<EnemyHealth>().addStack(1);
            }
            collidedObject = other.gameObject;
            collidedObject.GetComponent<EnemyHealth>().takeDamage(damage);
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
    }                                                                                     
