using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadTrack : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject boss;
    private Vector3 bossPosition;
    private float timer;
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Player");

        bossPosition = boss.transform.position;
        Vector3 direction = bossPosition - transform.position;
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rot + 0); //+0 is the rotation. EX: +90 is rotate 90 deg
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //bullet lasts 10 seconds before it dies
        if(timer>10)
        {
            Destroy(gameObject);
        }
    }
}
