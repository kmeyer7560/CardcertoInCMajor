using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public GameObject content;
    public GameObject pickScreen;
    private int random;
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
            if(random <= 40)
            {
                //roll 1
                ActivateContent();
                PickCard(1);
            }
            else if(random <= 60)
            {
                //roll 2
                ActivateContent();
                PickCard(2);
            }
            else if(random <= 80)
            {
                //roll 3
                ActivateContent();
                PickCard(3);
            }
            else if(random <= 90)
            {
                //roll 5
                ActivateContent();
                PickCard(5);
            }
            else
            {
                //roll 8
                ActivateContent();
                PickCard(8);
            }
        }
    }

    private void ActivateContent()
    {
        content.SetActive(true);
        animator.SetTrigger("roll5");
    }

    private void PickCard(int cards)
    {
        pickScreen.SetActive(true);
        
    }
}
