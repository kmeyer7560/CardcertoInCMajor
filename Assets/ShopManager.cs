using UnityEngine;
using UnityEngine.UI;

public class ShopMenuManager : MonoBehaviour
{
 /*   public ShopItemSlot[] itemSlots;
    public Button buyButton;
    private int selectedItemIndex = -1;

    void Start()
    {
        DeselectAllSlots();
        buyButton.interactable = false;
    }

    public void SelectItem(int index)
    {
        DeselectAllSlots();

        selectedItemIndex = index;
        itemSlots[index].Select();
        buyButton.interactable = true;
    }

    public void DeselectAllSlots()
    {
        foreach (var slot in itemSlots)
        {
            slot.Deselect();
        }

        selectedItemIndex = -1;
        buyButton.interactable = false;
    }

    public void BuySelectedItem()
    {
        if (selectedItemIndex == -1) return;

        var selectedItem = itemSlots[selectedItemIndex];
        Debug.Log("Bought: " + selectedItem.itemName);

        //Add logic for deducting currency, adding to inventory, etc.

        DeselectAllSlots();
    }
*/}
