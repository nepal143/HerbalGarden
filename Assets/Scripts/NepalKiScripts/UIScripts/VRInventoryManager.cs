using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VRInventoryManager : MonoBehaviour
{
    [Header("Hand Controller Parent")]
    public Transform handControllerParent; // Parent object containing all hand meshes

    [Header("Inventory Items")]
    public List<InventoryItem> inventoryItems = new List<InventoryItem>(); // List of items

    public Transform inventoryPanel; // The inventory parent where items exist as empty objects
    public Transform itemSpawnPoint; // Where the item should appear in hand

    public GameObject currentItem; // The currently equipped item

    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;       // Name of the item (should match UI image name)
        public GameObject itemPrefab; // 3D model prefab
        public GameObject itemImagePrefab; // UI prefab of RawImage
    }

    void Start()
    {
        GenerateInventoryUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) // Detects when left mouse button is released
        {
            DeselectItem();
        }
    }

    // Finds empty inventory slots and spawns UI images inside them
    public void GenerateInventoryUI()
    {
        int i = 0; // Track assigned inventory slots
        foreach (Transform child in inventoryPanel) // Loop through inventory slots
        {
            if (i >= inventoryItems.Count) break; // Stop if there are no more items to assign

            InventoryItem item = inventoryItems[i]; // Get corresponding item

            if (child.childCount == 0) // If slot is empty
            {
                GameObject uiElement = Instantiate(item.itemImagePrefab, child); // Spawn UI image inside slot
                
                // Add the ItemSelector script to the UI element (RawImage)
                ItemSelector itemSelector = uiElement.AddComponent<ItemSelector>();
                itemSelector.itemName = item.itemName; // Set the item name
                itemSelector.inventoryManager = this; // Set the reference to the inventory manager

                Button button = uiElement.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => SelectItem(item.itemName)); // Add listener for button click
                }
            }
            i++;
        }
    }

    // Function to select an item by name
    public void SelectItem(string itemName)
    {
        // Remove the previous item
        if (currentItem != null)
        {
            Destroy(currentItem);
        }

        // Find the matching item
        InventoryItem selectedItem = inventoryItems.Find(item => item.itemName == itemName);
        if (selectedItem != null && selectedItem.itemPrefab != null)
        {
            // Spawn the new item in hand
            currentItem = Instantiate(selectedItem.itemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
            currentItem.transform.parent = itemSpawnPoint; // Attach to hand

            // Hide all controller meshes
            ToggleHandMeshes(false);
        }
        else
        {
            Debug.LogError($"Item {itemName} not found!");
        }
    }

    // Function to deselect the current item
    public void DeselectItem()
    {
        if (currentItem != null)
        {
            Destroy(currentItem); // Remove the current item
            currentItem = null;
            ToggleHandMeshes(true); // Show hand meshes again
        }
    }

    // Enable/Disable all child meshes under the hand controller parent
    public void ToggleHandMeshes(bool state)
    {
        if (handControllerParent == null) return;

        foreach (Transform child in handControllerParent)
        {
            if (child.gameObject.activeSelf != state)
            {
                child.gameObject.SetActive(state);
            }
        }
    }
}