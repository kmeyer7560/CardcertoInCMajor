using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class itemSlot : MonoBehaviour
{
    //Item Data
    public Sprite itemSprite;
    public bool isFull;
    public string itemName;

    //Item Slot
    [SerializeField]
    private Image itemImage;
    
    private InventoryManager inventoryManager;
    void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(string itemName, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        isFull = true;

        itemImage.sprite = itemSprite;  
    }
}
