using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class EnemyScript : MonoBehaviour
{
    //https://www.youtube.com/watch?v=--u20SaCCow
    public GameObject bullet;
    public Transform bulletPos;
    
    private float timer;
    public GameObject player;
    public int fireRate = 2;
    
    private bool shooting;
    public float speed = 10;
    private Transform playerTarget;
    public int timeToShoot;

    private float startTime = Time.realtimeSinceStartup;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shooting = false;
        playerTarget = player.GetComponent<Transform>();
    }
    void Update()
    {
        UpdateWalking();
        float distance = Vector2.Distance(transform.position, player.transform.position);
        
        if(distance<10)
        {
            timer += Time.deltaTime;
            
            if(timer > fireRate)
            {
                timer = 0;
                Shoot();
            }
        }
    }

    void UpdateWalking()
    {
        if(shooting){

        }
        if(Time.realtimeSinceStartup != timeToShoot && shooting)
        {
            startTime += Time.deltaTime;

        }
        if(!shooting)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);
        }
    }
    void Shoot()
    {
        shooting = true;
        Instantiate(bullet, bulletPos.position, Quaternion.identity);

    }
}