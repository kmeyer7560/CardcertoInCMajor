using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool hasBeenPlayed;
    public int handIndex;

    private HandManager hm;

    public GameObject player;

    public GameObject staminaBar;



    private void Start()
    {
        hm = FindObjectOfType<HandManager>();
    }

    public void playCard()
    {
        if(!hasBeenPlayed)
        {
            staminaBar.GetComponent<StaminaManager>().useCard(20);
            hasBeenPlayed = true;
            hm.availableCardSlots[handIndex] = true;
            hm.hold.Add(this);
            hm.hand.Remove(this);
            hm.DrawCard();
            player.GetComponent<TopDownMovement>().Dash(50); //what the card should do should go here.
            hm.shuffle();
            hasBeenPlayed = false;
            gameObject.SetActive(false);
            
        }
    }
}
