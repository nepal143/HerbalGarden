using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for pointer events

public class ItemSelector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string itemName; // Name of the item (should match the inventory item)
    public VRInventoryManager inventoryManager; // Reference to the inventory manager

    void Start()
    {
        if (inventoryManager == null) 
        {
            // If inventoryManager is not assigned in the Inspector, get it using the "Inventory" tag
            inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<VRInventoryManager>();
        }
    }

    // Called when the button is pressed
    public void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryManager != null)
        {
            inventoryManager.SelectItem(itemName);
        }
        else
        {
            Debug.LogError("InventoryManager not found!");
        }
    }

    // Called when the button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        if (inventoryManager != null)
        {
            inventoryManager.DeselectItem();
        }
    }
}
