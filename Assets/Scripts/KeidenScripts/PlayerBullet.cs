using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBullet : MonoBehaviour
{
    public float Speed;

    public GameObject Player;

    Rigidbody2D rb;

    public Transform target;

    private float timer;

    public Transform objectInRay;

    bool inRange;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        target = FindClosestEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            objectInRay = Player.GetComponent<FOV>().hitObject;
            if(objectInRay == target)
            {
                inRange = true;
            } 
            else
            {
                Vector2.MoveTowards(transform.position, transform.forward, Speed * Time.deltaTime);
                inRange = false;
            }
        }

        if (inRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, Speed * Time.deltaTime);
        }



        Speed += 0.1f;

        timer += Time.deltaTime;
        // Bullet lasts 10 seconds before it dies
        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }
}                                                                                               
