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
            //getcomponent contentroll
            
        }
    }
}
