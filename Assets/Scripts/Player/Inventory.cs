using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();

    private int currentItemIndex = 0;

    public void CycleItems(InputAction.CallbackContext context)
    {
        int direction = Mathf.RoundToInt(context.ReadValue<Vector2>().y);
        if(items.Count != 0) currentItemIndex = (currentItemIndex + direction + items.Count) % items.Count;

        Debug.Log("Current item index after cycling: " + currentItemIndex);
        
    }
    public void setcurrentItemIndex(int index)
    {
        currentItemIndex = index;
        Debug.Log("Current item index set to: " + currentItemIndex);
    }

    public void AddItem(string itemName, int itemKey, Image inventoryImage)
    {
        // Logic to add item to the inventory
        Debug.Log("Item added: " + itemName + " with key: " + itemKey);
        items.Add(new Item(itemName, itemKey, inventoryImage));
        // Here you would typically add the item to a list or dictionary
    }

    public void RemoveItem()
    {
        if (items.Count > 0)
        {
            items.RemoveAt(currentItemIndex);
        }
    }

    public void UseItem(int itemKey)
    {
        //When the item is used, check if the interactable is not null
        //Check if the item key is the same as the itemKey of the interactable
        //if both checks pass, remove the item from the inventory and call the interactable's Interact method.
    }
}
