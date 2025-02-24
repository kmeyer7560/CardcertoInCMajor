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
    public GameObject slash;
    public float staminaCost;
    public string cardType;
    public float dashStrength;
    public float defense;
    public int burstShotNum;
    public List<GameObject> eneimes;
    public int deflectBullets;


    private void Start()
    {
        hm = FindObjectOfType<HandManager>();
        cardType = this.tag;
    }

    public void playCard()
    {
        if (!hasBeenPlayed)
        {
            if (staminaBar.GetComponent<StaminaManager>().stamina >= staminaCost)
            {
                staminaBar.GetComponent<StaminaManager>().useCard((int) staminaCost);
                hasBeenPlayed = true;
                hm.availableCardSlots[handIndex] = true;
                hm.hold.Add(this);
                hm.hand.Remove(this);
                hm.DrawCard();

                if (cardType == "dashCard")
                {
                    dashCard();
                }
                if (cardType == "gDashCard") //guitar dash
                {
                    gDash();
                }
                else if (cardType == "attackCard") //simple attack
                {
                    shootCard();
                }
                else if (cardType == "burstCard") //any burst shot
                {
                    gameObject.SetActive(true);
                    StartCoroutine(burstDelay(burstShotNum));
                }
                else if (cardType == "laserCard") //guitar laser
                {
                    player.GetComponent<PlayerMovement>().moveable = false;
                    player.GetComponent<PlayerMovement>().rb.velocity = new Vector2(0, 0);;
                    shootCard();
                }
                else if (cardType == "defenseCard") //guitar defense increase
                {
                    healthBar.GetComponent<PlayerHealthBar>().setDefense(defense);
                }
                else if (cardType == "speedCard") //violin speedup
                {
                    player.GetComponent<PlayerMovement>().speedUp();
                }
                else if (cardType == "blowCard") //violin detonate
                {
                    List <GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
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
                    healthBar.GetComponent<PlayerHealthBar>().deflect(deflectedValue => StartCoroutine(OnDeflectComplete(deflectedValue)));
                    healthBar.GetComponent<PlayerHealthBar>().deflectedNum = 0;
                }


                hm.shuffle();
                hasBeenPlayed = false;
                if (cardType != "burstCard" && cardType != "deflectCard") //need this for every card that uses a coroutine f you unity
                {
                    gameObject.SetActive(false);
                }

            }
        }
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
    IEnumerator wait(double wait)
    {
        yield return new WaitForSeconds((float) wait);
    }

    IEnumerator OnDeflectComplete(int deflectedValue)
{
    Debug.Log("Deflected Number: " + deflectedValue);
    GameObject closestEnemy = FindClosestEnemy();
    if (closestEnemy != null)
    {   
        slash.SetActive(true); // Activate the slash GameObject once before the loop
        for (int i = 0; i < deflectedValue; i++)
        {
            slash.transform.position = closestEnemy.transform.position; // Move the slash to the enemy's position
            slash.transform.rotation = Random.rotation;
            slash.GetComponent<Animator>().Play("Slash"); // Play the slash animation
            
            // Wait for the duration of the animation before playing it again
            yield return new WaitForSeconds(0.125f); // Adjust this value based on the length of your animation
            
            closestEnemy.GetComponent<EnemyHealth>().deflectSlash(); // Call the deflect method on the enemy
        }
        slash.SetActive(false); // Deactivate the slash GameObject after all animations
    }
    healthBar.GetComponent<PlayerHealthBar>().deflectedNum = 0;
    gameObject.SetActive(false);
}

    private GameObject FindClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
    {
        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
    }

    return closestEnemy;
}


    public void dashCard()
    {
        player.GetComponent<PlayerMovement>().dash(dashStrength, false);

    }

    public void gDash()
    {
        player.GetComponent<PlayerMovement>().dash(dashStrength, true);
    }

    public void shootCard()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
    }
}
