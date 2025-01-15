using System.Runtime.CompilerServices;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public GameObject content;
    public GameObject pickScreen;
    private int random;
    private int randomSuit;
    private int randomCard;
    private int randomSpecialCard;
    private int amountOfCards;
    private Animator animator;

    void Start()
    {
        content = GameObject.Find("Content");
        pickScreen = GameObject.Find("PickScreen");
        animator = content.GetComponent<Animator>();
        content.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Chest") && Input.GetKeyDown(KeyCode.Space))
        {
            random = Random.Range(0, 100);
            randomSuit = Random.Range(0,3);
            if(random <= 39)
            {
                //roll 2
                ActivateContent();
                SpawnCards(2);
            }
            else if(random <= 59)
            {
                //roll 4
                ActivateContent();
                SpawnCards(4);
            }
            else if(random <= 89)
            {
                //roll 8
                ActivateContent();
                SpawnCards(8);
            }
            else
            {
                //roll special
                ActivateContent();
                SpawnCards(1);
            }
        }
    }

    private void ActivateContent()
    {
        content.SetActive(true);
        animator.SetTrigger("roll5");
    }

    private void SpawnCards(int cards)
    {
        int randomCard = Random.Range(0,3);
        //int random
        //if(cards == 1)
        //{
          //  specialCards[]
        //}
        //pickScreen.SetActive(true);
        
    }
}
