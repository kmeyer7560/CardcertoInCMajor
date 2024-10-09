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
    public double fireRate = 2.0;
    private bool shooting;
    public float speed = 10;
    private Transform playerTarget;
    public int timeToShoot = 2;
    private double startTime;
    public int range = 10;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shooting = false;
        playerTarget = player.GetComponent<Transform>();
        startTime = Time.realtimeSinceStartup;
    }
    void Update()
    {
        UpdateWalking();
        float distance = Vector2.Distance(transform.position, player.transform.position);
        
        if(distance<=range)
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
        if(shooting)
        {
            if(Time.realtimeSinceStartup >= startTime + timeToShoot)
            {
                timeToShoot--;
                if(timeToShoot<= 0)
                {
                    shooting = false;
                }
            }
            startTime = Time.realtimeSinceStartup;
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