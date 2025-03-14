using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random=UnityEngine.Random;

public class ShotgunEnemy : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public GameObject player;
    public float fireRate = 2.0f;
    public float speed = 10f;
    public float shootDuration = 2f;
    public float range = 10f;

    private bool shooting;
    private Transform playerTarget;
    private float shootStartTime;
    private float lastShotTime;

    public Vector3 direction = Vector3.zero;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shooting = false;
        playerTarget = player.GetComponent<Transform>();
        
        lastShotTime = -fireRate;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        
        if (distance <= range)
        {
            if (!shooting)
            {

                //august's non gorped code
                if ((int)(Time.time % fireRate) == 0)
                {
                    StartShooting();
                }
            }
            else
            {
                if (Time.time - shootStartTime >= shootDuration)
                {
                    StopShooting();
                }
            }
        }
        else
        {
            StopShooting();
        }
        
        if (!shooting)
        {
            MoveTowardsPlayer();
        }
    }

    void StartShooting()
    {
        shooting = true;
        shootStartTime = Time.time;
        Shoot();

    }

    void StopShooting()
    {
        shooting = false;
    }


    public void Shoot() {
        int bulletCount = 10;
        float spreadAngle = 30f;
        float halfSpread = spreadAngle / 2f;

        for (int i = 0; i < bulletCount; i++) {
            float spread = Random.Range(-halfSpread, halfSpread);
            Quaternion newRot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + spread);
            Instantiate(bullet, bulletPos.position, newRot);
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);
    }

    void Strafe()
    {
        transform.position = Vector2.MoveTowards(transform.position,direction, speed * Time.deltaTime);
    }
}