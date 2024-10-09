using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public Transform[] cardSlots;
    public List<Card> hand = new List<Card>();
    public bool[] availableCardSlots;

    private Card lastcard;
    private int turnsSinceCard = 0;

    

    public void DrawCard()
    {
        if (deck.Count >= 1){
            Card randCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if(availableCardSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                    randCard.transform.position = cardSlots[i].position;
                    availableCardSlots[i] = false;
                    hand.Add(randCard);



                   /* lastcard = randCard;
                    turnsSinceCard++;
                    deck.Remove(randCard);
                    if (turnsSinceCard == 1)
                    {
                        turnsSinceCard = 0;
                        deck.Add(lastcard);
                    } */
                    return;
                }
            }
        }
    }



    void Start()
    {
        for (int i = 0; i <4; i++)
        {
            DrawCard();
        }
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            hand[0].GetComponent<Card>().playCard();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            hand[1].GetComponent<Card>().playCard();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            hand[2].GetComponent<Card>().playCard();
        }

        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            hand[3].GetComponent<Card>().playCard();
        }
    }

}
