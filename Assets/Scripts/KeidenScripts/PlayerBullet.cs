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
        Debug.Log(Player.GetComponent<FOV>().hitObject);
        objectInRay = Player.GetComponent<FOV>().hitObject;
        Debug.Log(objectInRay);
        if(objectInRay == null){ //ian code
            rb.velocity = Player.GetComponent<PlayerMovement>().savedDirection * Speed;      
        }

        Debug.Log(Player.GetComponent<Rigidbody2D>().velocity);
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
        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            collidedObject = other.gameObject;
            collidedObject.GetComponent<EnemyHealth>().takeDamage(damage);
        }
        
        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    }                                                                                     
