using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

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
    public float staminaCost;
    public string cardType;
    public float dashStrength;
    public float defense;
    public int burstShotNum;


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
                staminaBar.GetComponent<StaminaManager>().useCard(staminaCost);
                hasBeenPlayed = true;
                hm.availableCardSlots[handIndex] = true;
                hm.hold.Add(this);
                hm.hand.Remove(this);
                hm.DrawCard();

                if (cardType == "dashCard")
                {
                    dashCard();
                }
                if (cardType == "gDashCard")
                {
                    gDash();
                }
                else if (cardType == "attackCard")
                {
                    shootCard();
                }
                else if (cardType == "burstCard")
                {
                    gameObject.SetActive(true);
                    StartCoroutine(burstDelay(burstShotNum));
                }
                else if (cardType == "laserCard")
                {
                    player.GetComponent<PlayerMovement>().moveable = false;
                    player.GetComponent<PlayerMovement>().rb.velocity = new Vector2(0, 0);;
                    shootCard();
                }
                else if (cardType == "defenseCard")
                {
                    healthBar.GetComponent<PlayerHealthBar>().setDefense(defense);
                }
                else if (cardType == "speedCard")
                {
                    player.GetComponent<PlayerMovement>().speedUp();
                }

                hm.shuffle();
                hasBeenPlayed = false;
                if (cardType != "burstCard") //need this for every card that uses a coroutine f you unity
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                staminaBar.GetComponent<StaminaManager>().ChargeRate = 10;
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
            yield return new WaitForSeconds(0.1f); // Wait for 0.3 seconds before the next shot
        }
        gameObject.SetActive(false);
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
