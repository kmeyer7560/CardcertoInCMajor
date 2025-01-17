using System.Runtime.CompilerServices;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public GameObject content2;
    public GameObject content4;
    public GameObject content8;
    public GameObject contentSpecial;
    public GameObject pickScreen;
    private int random;
    private int randomSuit;
    private int randomCard;
    private int randomSpecialCard;
    private int amountOfCards;
    private Animator animator;

    void Start()
    {
        content2 = GameObject.Find("Content2");
        content4 = GameObject.Find("Content2");
        content8 = GameObject.Find("Content2");
        contentSpecial = GameObject.Find("Content2");

        animator = content2.GetComponent<Animator>();

        content2.SetActive(false);
        content4.SetActive(false);
        content8.SetActive(false);
        contentSpecial.SetActive(false);
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
                if(randomSuit == 0)
                {

                }
                ActivateContent(2);
                SpawnCards(2);
            }
            else if(random <= 59)
            {
                //roll 4
                ActivateContent(4);
                SpawnCards(4);
            }
            else if(random <= 89)
            {
                //roll 8
                ActivateContent(8);
                SpawnCards(8);
            }
            else
            {
                //roll special
                ActivateContent(1);
                SpawnCards(1);
            }
        }
    }

    private void ActivateContent(int roll)
    {
        if(roll == 1)
        {
            contentSpecial.SetActive(true);
        }
        
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
