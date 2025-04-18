using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random=UnityEngine.Random;

public class TractorDriver : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public GameObject player;
    public float fireRate = 2.0f;
    public float speed = 10f;
    public float shootDuration = 2f;
    public float range = 10f;

    public float spreadAmount;

    private bool shooting;
    private Transform playerTarget;
    private float shootStartTime;
    private float lastShotTime;

    public Vector3 direction = Vector3.zero;

    private bool burst;
    public float burstrate = 1.0f;
    void Start()
    {
        //player is the object with the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        //automatically makes shooting false so they dont shoot before being in range
        shooting = false;
        //sets the player's transform component necessary for knowing where the player is
        playerTarget = player.GetComponent<Transform>();
        lastShotTime = -fireRate;
    }

    void Update()
    {
        if(!burst)
        {
            if((int)(Time.time % burstrate) ==0)
            {
                burst = true;
            }
        }
        if(burst)
        {
            if((int)(Time.time % burstrate) ==0)
            {
                burst = false;
            }
        }



        //updates distance from player
        float distance = Vector2.Distance(transform.position, player.transform.position);
        
        //if distance is less then set range
        if (distance <= range)
        {
            //if not shooting
            if (!shooting)
            {

                //august's non gorped code
                if ((int)(Time.time % fireRate) == 0)
                {
                    StartShooting();
                }
            }
            //else if shooting
            else
            {
                //calculates shoot duration. Current time since the start of the game - when the enemy starts shooting >= set shoot duration variable. EX: if the shoot duration is 2 then stop after two seconds until told to start shooting again.
                if (Time.time - shootStartTime >= shootDuration)
                {
                    StopShooting();
                }
            }
        }
        //else if enemy is out of range
        else
        {
            StopShooting();
        }
        
        //if not shooting then move towards player
        if (!shooting)
        {
            MoveTowardsPlayer();
        }
    }

    void StartShooting()
    {
        shooting = true;
        //shootStartTime = current time since started the game
        shootStartTime = Time.time;
        if(burst)
        {
            Shoot();    
        }
    }

    void StopShooting()
    {
        shooting = false;
        // timer resets for shooting
    }

    void Shoot()
    {
        if (Time.time - lastShotTime >= fireRate)
        {
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
            lastShotTime = Time.time;
            direction.x = Random.Range(-1,2);
            direction.y = Random.Range(-1,2);
            Strafe();

        }
    }

    void MoveTowardsPlayer()
    {
        //calculate where the  player is
        //Debug.Log("move");
        //Vector2 directionToPlayer = (playerTarget.position - transform.position).normalized;
        //move towards the player
        transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);
    }

    void Strafe()
    {
        transform.position = Vector2.MoveTowards(transform.position,direction, speed * Time.deltaTime);
    }
}