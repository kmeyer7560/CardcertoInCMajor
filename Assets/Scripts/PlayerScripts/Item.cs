using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    [SerializeField]
    public Card card;
    [SerializeField]
    private string itemName;
    
    [SerializeField]
    private Sprite sprite;

    [TextArea]
    [SerializeField]
    private string itemDescription;

    private InventoryManager inventoryManager;
    public GameObject hm;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision) //this is what triggers it to enter the inventory.
    {
        if(collision.gameObject.tag == "Player")
        {
            inventoryManager.AddItem(itemName,sprite,itemDescription);
            Card madeobject = Instantiate(card);
            madeobject.transform.SetParent(GameObject.Find("Canvas").transform);
            hm.GetComponent<HandManager>().deck.Add(madeobject);
            Destroy(gameObject);
        }
    }
}
