using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();

    private int currentItemIndex = 0;

    InventoryAction actions;

    private bool isInventoryOpen = false;

    public GameObject inventoryUI; // Reference to the inventory UI GameObject

    private TextMeshProUGUI itemNameText; // Reference to the TextMeshProUGUI for item name display 

    private Player player;

    public void Awake()
    {
        actions = new InventoryAction();
        actions.Inventory.InventoryToggle.performed += _ => ToggleInventory();
        actions.Inventory.CycleItems.performed += CycleItems;
        actions.Enable();
    }

    public void Start()
    {
        Transform InventoryPanelTransform = inventoryUI.transform.Find("Panel");
        GameObject panel = InventoryPanelTransform != null ? InventoryPanelTransform.gameObject : null;
        Transform itemNameTransform = panel != null ? panel.transform.Find("ItemName") : null;
        itemNameText = itemNameTransform != null ? itemNameTransform.GetComponent<TextMeshProUGUI>() : null;
        inventoryUI.SetActive(false);
        player = FindAnyObjectByType<Player>();
    }

    public void OnEnable()
    {
        actions.Enable();
    }

    public void OnDisable()
    {
        actions.Disable();
    }

    public void ToggleInventory()
    {
        //Logic to toggle the inventory UI
        //unlock the cursor
        Debug.Log("Inventory toggled");
        if (isInventoryOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }

    }

    private void OpenInventory()
    {
        isInventoryOpen = true;
        // Show the inventory UI
        Debug.Log("Inventory opened");
        inventoryUI.SetActive(true);
        Cursor.visible = true; // Make the cursor visible
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        //disable player movement
        if (player != null)
        {
            player.playerInput.actions.Disable(); // Disable player input actions
        }

        //Set the item name text to the last item seen before closing the inventory
        if (items.Count > 0)
        {
            itemNameText.text = items[currentItemIndex].itemName; // Update the item name text
        }
        else
        {
            itemNameText.text = ""; // Default text if no items
        }
    }

    private void CloseInventory()
    {
        isInventoryOpen = false;
        // Hide the inventory UI
        Debug.Log("Inventory closed");
        inventoryUI.SetActive(false);
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        if (player != null)
        {
            player.playerInput.actions.Enable(); // Re-enable player input actions
        }
    }

    public void CycleItems(InputAction.CallbackContext context)
    {
        int direction = Mathf.RoundToInt(context.ReadValue<Vector2>().y);
        if (items.Count != 0) currentItemIndex = (currentItemIndex + direction + items.Count) % items.Count;

        // Logic to cycle through items in the inventory
        if(items.Count > 0)
            itemNameText.text = items[currentItemIndex].itemName; // Update the item name text

        Debug.Log("Current item index after cycling: " + currentItemIndex);
    }
    public void setcurrentItemIndex(int index)
    {
        currentItemIndex = index;
        Debug.Log("Current item index set to: " + currentItemIndex);
    }

    public void AddItem(string itemName, int itemKey)
    {
        // Logic to add item to the inventory
        Debug.Log("Item added: " + itemName + " with key: " + itemKey);
        items.Add(new Item(itemName, itemKey));
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
