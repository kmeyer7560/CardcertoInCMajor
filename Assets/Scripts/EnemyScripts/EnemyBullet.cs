using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force;
    private float timer;
    
    void Start()
    {
        //declare rigidbody
        rb = GetComponent<Rigidbody2D>();
        //set player as the object with the "Player" tag
        player = GameObject.FindGameObjectWithTag("Player");
        
        //calculate direction
        Vector3 direction = player.transform.position - transform.position;
        //calculate velocity
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        
        //calculate rotation so bullet always points at player
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rot + 0); //+0 is the rotation. EX: +90 is rotate 90 deg
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        //bullet lasts 10 seconds before it dies
        if(timer>10)
        {
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //subtract player health here
        }
        Destroy(gameObject);
    }
}
