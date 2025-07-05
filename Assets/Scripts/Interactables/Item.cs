using UnityEngine.UI;

public struct Item
{
    public string itemName { get; private set; }

    public int itemKey { get; private set; }

    public Image inventoryImage { get; private set; }
    
    public Item(string name, int key, Image image)
    {
        itemName = name;
        itemKey = key;
        inventoryImage = image;
    }
}
