using UnityEngine;
using UnityEngine.UI;

public class InventoryInteractable : Interactable
{

    Item item;

    [SerializeField] private string itemName;
    [SerializeField] private int itemKey;
    [SerializeField] private Image inventoryImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        item = new Item(itemName, itemKey, inventoryImage);
        Debug.Log("Item created: " + item.itemName + " with key: " + item.itemKey);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Player player)
    {
        player.inventory.AddItem(item.itemName, item.itemKey, item.inventoryImage);
    }
}
