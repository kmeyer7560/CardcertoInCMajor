using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBullet: MonoBehaviour
{
    public float Speed;

    Rigidbody2D rb;

    public Transform target;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position,Speed * Time.deltaTime);
        Speed += (float) 0.1;

        timer += Time.deltaTime;
        //bullet lasts 10 seconds before it dies
        if(timer>10)
        {
            Destroy(gameObject);
        }
    }
}
