using UnityEngine;
using UnityEngine.UI;

public class ItemInteractable : Interactable
{

    Item item;

    [SerializeField] private string itemName;
    [SerializeField] private int itemKey;
    //[SerializeField] private Texture2D inventoryImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        item = new Item(itemName, itemKey);
        Debug.Log("Item created: " + item.itemName + " with key: " + item.itemKey);
    }

    public override void Interact(Player player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null, cannot interact with item.");
            return;
        }
        else if (!player.inventory)
        {
            Debug.LogError("Player's inventory is null, cannot add item.");
            return;
        }
        else
        {
            player.inventory.AddItem(item.itemName, item.itemKey);
            Destroy(gameObject);
        }
    }
}
