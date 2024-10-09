using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool hasBeenPlayed;
    public int handIndex;

    private HandManager hm;


    private void Start()
    {
        hm = FindObjectOfType<HandManager>();
    }

    public void playCard()
    {
        if(!hasBeenPlayed)
        {
            transform.position += Vector3.down * 5;
            hasBeenPlayed = true;
            hm.availableCardSlots[handIndex] = true;

        }
    }
}
