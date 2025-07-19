using UnityEngine;
using UnityEngine.UI;

public struct Item
{
    public string itemName { get; private set; }

    public int itemKey { get; private set; }

    //public Texture2D inventoryImage { get; private set; }

    public Item(string name, int key)
    {
        itemName = name;
        itemKey = key;
        //inventoryImage = image;
    }
}
