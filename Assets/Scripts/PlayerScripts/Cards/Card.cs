using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random=UnityEngine.Random;
public class Card : MonoBehaviour
{
    public bool hasBeenPlayed;
    public int handIndex;
    private HandManager hm;
    public GameObject staminaBar;
    public GameObject player;
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public GameObject healthBar;
    public GameObject roomController;
    public GameObject violinSlashFX;
    public GameObject bam1;
    public GameObject bam2;
    public GameObject bam3;
    public GameObject windWall;
    public GameObject fSLash;
    public float staminaCost;
    public string cardType;
    public float dashStrength;
    public float defense;
    public int burstShotNum;
    public List<GameObject> eneimes;
    public int deflectBullets;
    private Animator anim;
    public int type; //0 = universals, 1 = guitar, 2 = violin, 3 = drum

    public AudioClip Guitar1;
    public AudioClip Guitar2;
    public AudioClip Guitar3;
    public AudioClip Violin1;
    public AudioClip Violin2;
    public AudioClip Violin3;
    public AudioClip Flute1;
    public AudioClip Flute2;
    public AudioClip Flute3;
    public AudioClip Drum1;
    public AudioClip Drum2;
    public AudioClip Drum3;
    public AudioClip Magic;
    private AudioSource audioSource;



    private void Start()
    {
        audioSource = GameObject.Find("SoundFX").GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("audiosource null");
        }
        hm = FindObjectOfType<HandManager>();
        anim = FindObjectOfType<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        shootingPoint = GameObject.FindGameObjectWithTag("Player").transform;
        staminaBar = GameObject.FindGameObjectWithTag("stamina");
        roomController = GameObject.FindGameObjectWithTag("roomController");
        fSLash = GameObject.FindGameObjectWithTag("fSlash");
        windWall = GameObject.Find("WindWall");
        cardType = this.tag;
        gameObject.transform.position = new Vector2(1000000, 100000);
        if (this.CompareTag("fSlashCard"))
        {
            fSLash.GetComponent<fluteSlash>().isSlash = true;
            fSLash.SetActive(false);
        }
        if (this.CompareTag("fDefenseCard"))
        {
            windWall.SetActive(false);
        }
        hm.GetComponent<HandManager>().deck.Add(this);
        hm.GetComponent<HandManager>().DrawCard();
    }

    public void playCard()
    {
        if (!hasBeenPlayed)
        {
            if (staminaBar.GetComponent<StaminaManager>().stamina >= staminaCost)
            {
                if (type == 1)
                {
                    //anim.SetTrigger("guitarAttack");
                    player.GetComponent<PlayerMovement>().stupidDumbassFunction();
                }
                staminaBar.GetComponent<StaminaManager>().useCard((int) staminaCost);
                hasBeenPlayed = true;
                hm.availableCardSlots[handIndex] = true;
                hm.hold.Add(this);
                hm.hand.Remove(this);
                hm.DrawCard();
                Debug.Log("card played");
                if (cardType == "dashCard")
                {
                    dashCard();
                    if (type == 0)
                    {
                        audioSource.PlayOneShot(Magic);
                    }
                }
                else if (cardType == "gDashCard") //guitar dash
                {
                    audioSource.PlayOneShot(new AudioClip[] { Guitar1, Guitar2, Guitar3 }[Random.Range(0, 3)]);
                    gDash();
                }
                else if (cardType == "attackCard") //simple attack
                {
                    shootCard();
                    if (type == 0)
                    {
                        audioSource.PlayOneShot(Magic);
                    }
                    else if (type == 1)
                    {
                        audioSource.PlayOneShot(new AudioClip[] { Guitar1, Guitar2, Guitar3 }[Random.Range(0, 3)]);
                    }
                    else if (type == 2)
                    {
                        audioSource.PlayOneShot(new AudioClip[] { Violin1, Violin2, Violin3 }[Random.Range(0, 3)]);
                    }
                    else if (type == 3)
                    {
                        audioSource.PlayOneShot(new AudioClip[] { Drum1, Drum2, Drum3 }[Random.Range(0, 3)]);
                    }
                }
                else if (cardType == "burstCard") //any burst shot
                {
                    gameObject.SetActive(true);
                    StartCoroutine(burstDelay(burstShotNum));
                    if (type == 1)
                    {
                        audioSource.PlayOneShot(new AudioClip[] { Guitar1, Guitar2, Guitar3 }[Random.Range(0, 3)]);
                    }
                    else if (type == 2)
                    {
                        audioSource.PlayOneShot(new AudioClip[] { Violin1, Violin2, Violin3 }[Random.Range(0, 3)]);
                    }
                }
                else if (cardType == "laserCard") //guitar laser
                {
                    audioSource.PlayOneShot(new AudioClip[] { Guitar1, Guitar2, Guitar3 }[Random.Range(0, 3)]);
                    player.GetComponent<PlayerMovement>().moveable = false;
                    player.GetComponent<PlayerMovement>().rb.velocity = new Vector2(0, 0); ;
                    shootCard();
                }
                else if (cardType == "GdefenseCard") //guitar defense increase
                {
                    audioSource.PlayOneShot(new AudioClip[] { Guitar1, Guitar2, Guitar3 }[Random.Range(0, 3)]);
                    healthBar.GetComponent<PlayerHealthBar>().setDefense(defense, 1);
                }
                else if (cardType == "speedCard") //violin speedup
                {
                    player.GetComponent<PlayerMovement>().speedUp();
                    audioSource.PlayOneShot(new AudioClip[] { Violin1, Violin2, Violin3 }[Random.Range(0, 3)]);
                }
                else if (cardType == "blowCard") //violin detonate
                {
                    audioSource.PlayOneShot(new AudioClip[] { Violin1, Violin2, Violin3 }[Random.Range(0, 3)]);
                    List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
                    foreach (GameObject i in enemies)
                    {
                        if (i == null)
                        {
                            enemies.Remove(i);
                        }
                        i.GetComponent<EnemyHealth>().detonate();
                    }
                }
                else if (cardType == "deflectCard")
                {
                    gameObject.transform.position = new Vector2(1000000, 100000);
                    healthBar.GetComponent<PlayerHealthBar>().deflect(deflectedValue => StartCoroutine(OnDeflectComplete(deflectedValue)));
                    healthBar.GetComponent<PlayerHealthBar>().deflectedNum = 0;
                    audioSource.PlayOneShot(new AudioClip[] { Violin1, Violin2, Violin3 }[Random.Range(0, 3)]);
                }
                else if (cardType == "DdefenseCard")
                {
                    healthBar.GetComponent<PlayerHealthBar>().setDefense(defense, 2);
                    audioSource.PlayOneShot(new AudioClip[] { Drum1, Drum2, Drum3 }[Random.Range(0, 3)]);
                }
                else if (cardType == "drumBamCard")
                {
                    audioSource.PlayOneShot(new AudioClip[] { Drum1, Drum2, Drum3 }[Random.Range(0, 3)]);
                    Debug.Log("drumbam started");
                    StartCoroutine(DrumBam());
                }
                else if (cardType == "reinDashCard")
                {
                    audioSource.PlayOneShot(new AudioClip[] { Drum1, Drum2, Drum3 }[Random.Range(0, 3)]);
                    dDash();
                }

                else if (cardType == "fDefenseCard")
                {
                    audioSource.PlayOneShot(new AudioClip[] { Flute1, Flute2, Flute3 }[Random.Range(0, 3)]);
                    windWall.transform.position = new Vector2(10000, 10000);
                    windWall.SetActive(true);
                    windWall.GetComponent<windWall>().activate();
                }
                else if (cardType == "fDashCard")
                {
                    audioSource.PlayOneShot(new AudioClip[] { Flute1, Flute2, Flute3 }[Random.Range(0, 3)]);
                    fDash();
                }
                else if (cardType == "fSlashCard")
                {
                    audioSource.PlayOneShot(new AudioClip[] { Flute1, Flute2, Flute3 }[Random.Range(0, 3)]);
                    fSLash.transform.position = new Vector2(1000, 1000);
                    fSLash.SetActive(true);
                    fSLash.GetComponent<fluteSlash>().activate();
                }
                else if (cardType == "getOverHereCard")
                {
                    audioSource.PlayOneShot(new AudioClip[] { Flute1, Flute2, Flute3 }[Random.Range(0, 3)]);
                    FindClosestEnemy().GetComponent<EnemyScript>().getHooked();
                }
                else if (cardType == "potofGreed")
                {
                    audioSource.PlayOneShot(Magic);
                    greed();
                }
                hm.shuffle();
                hasBeenPlayed = false;
                if (cardType != "burstCard" && cardType != "deflectCard" && cardType != "drumBamCard") //need this for every card that uses a coroutine f you unity
                {
                    gameObject.SetActive(false);
                }

    
            }
        }
    }

    IEnumerator DrumBam()
    {
            Instantiate(bam1, shootingPoint.position, shootingPoint.rotation);
            yield return new WaitForSeconds(0.3f);
            Instantiate(bam2, shootingPoint.position, shootingPoint.rotation);
            yield return new WaitForSeconds(0.3f);
            Instantiate(bam3, shootingPoint.position, shootingPoint.rotation);
            gameObject.SetActive(false);
    }
    IEnumerator burstDelay(int num)
    {
        gameObject.transform.position = new Vector2(1000000, 100000); //just to get the sprite out of the way becuase i can't turn off the gameobject while a coroutine needs to run. Very desperate tactic.
        //Debug.Log("Starting burst delay...");
        for (int i = 0; i < num; i++)
        {
            //Debug.Log("Shooting bullet " + (i + 1));
            shootCard(); // Call the shootCard method to instantiate a bullet
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds before the next shot
        }
        gameObject.SetActive(false);
    }

    IEnumerator OnDeflectComplete(int deflectedValue)
    {
        Debug.Log("Deflected Number: " + deflectedValue);
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            violinSlashFX.SetActive(true); // Activate the slash GameObject once before the loop
            for (int i = 0; i < deflectedValue; i++)
            {
                violinSlashFX.transform.position = closestEnemy.transform.position; // Move the slash to the enemy's position
                violinSlashFX.transform.rotation = Random.rotation;
                violinSlashFX.GetComponent<Animator>().Play("Slash"); // Play the slash animation

                // Wait for the duration of the animation before playing it again
                yield return new WaitForSeconds(0.125f); // Adjust this value based on the length of your animation

                closestEnemy.GetComponent<EnemyHealth>().deflectSlash(); // Call the deflect method on the enemy
            }
            violinSlashFX.SetActive(false); // Deactivate the slash GameObject after all animations
        }
        healthBar.GetComponent<PlayerHealthBar>().deflectedNum = 0;
        gameObject.SetActive(false);
}

   private GameObject FindClosestEnemy()
{
    GameObject closestEnemy = null;
    float closestDistance = Mathf.Infinity;

    // Get the current room of the player
    Room currentRoom = (roomController.GetComponent<RoomController>().currRoom); // Assuming you have a method to get the current room

    // Check if the current room is valid
    if (currentRoom == null)
    {
        Debug.LogWarning("Current room is null.");
        return null;
    }

    // Iterate through all enemies in the current room
    foreach (Transform child in currentRoom.transform)
    {
        if (child.CompareTag("Enemy"))
        {
            float distance = Vector3.Distance(player.transform.position, child.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = child.gameObject;
            }
        }
    }

    return closestEnemy;
}



    public void dashCard()
    {
        player.GetComponent<PlayerMovement>().dash(dashStrength, false, false);

    }

    public void gDash()
    {
        player.GetComponent<PlayerMovement>().dash(dashStrength, true, false);
    }

    public void fDash()
    {
        player.GetComponent<PlayerMovement>().dash(dashStrength, false, true);
    }
    public void dDash()
    {
        player.GetComponent<PlayerMovement>().reinDash();
    }

    public void shootCard()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
    }
    private void greed()
    {
        staminaBar.GetComponent<StaminaManager>().stamina += 3;
    }
}
