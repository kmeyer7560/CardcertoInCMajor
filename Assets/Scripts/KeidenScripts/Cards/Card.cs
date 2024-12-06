using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Dependencies.NCalc;
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
    public float staminaCost;
    public string cardType;
    public float dashStrength;
    private void Start()
    {
        hm = FindObjectOfType<HandManager>();
        cardType = this.tag;
        //Debug.Log(cardType);

    }

    public void playCard()
    {
        if(!hasBeenPlayed)
        {
            if (staminaBar.GetComponent<StaminaManager>().stamina >= staminaCost)
            {

                staminaBar.GetComponent<StaminaManager>().useCard(staminaCost);
                hasBeenPlayed = true;
                hm.availableCardSlots[handIndex] = true;
                hm.hold.Add(this);
                hm.hand.Remove(this);
                hm.DrawCard();
                if(cardType == "dashCard"){  //what the card should do should go here.
                    dashCard();
                }

                if(cardType == "attackCard"){
                    shootCard();
                }
                hm.shuffle();
                hasBeenPlayed = false;
                gameObject.SetActive(false);
            }
            else {
                staminaBar.GetComponent<StaminaManager>().ChargeRate = 10;
            }
            
        }
    }

    public void dashCard()
    {
        player.GetComponent<PlayerMovement>().Dash(dashStrength);
    }

    public void shootCard()
    {
        Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
    }

}
