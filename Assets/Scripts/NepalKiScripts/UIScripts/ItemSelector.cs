using UnityEngine;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour
{
    public string itemName; // Name of the item (should match the inventory item)

    // Make the inventoryManager public so it can be accessed by other scripts
    public VRInventoryManager inventoryManager; 

    void Start()
    {
        if (inventoryManager == null) 
        {
            // If inventoryManager is not assigned in the Inspector, get it using the "Inventory" tag
            inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<VRInventoryManager>();
        }
    }

    // This will be triggered when the item is clicked
    public void OnItemClicked()
    {
        if (inventoryManager != null)
        {
            inventoryManager.SelectItem(itemName); // Call the SelectItem function in the InventoryManager
        }
        else
        {
            Debug.LogError("InventoryManager not found!");
        }
    }
}
