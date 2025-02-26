using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VRInventoryManager : MonoBehaviour
{
    [Header("Hand Controller Parent")]
    public Transform handControllerParent;

    [Header("Inventory Items")]
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public Transform inventoryPanel;
    public Transform itemSpawnPoint;

    public GameObject currentItem;

    [System.Serializable]
    public class InventoryItem
    {
        public int index;
        public string itemName;
        public GameObject itemPrefab;
        public GameObject itemImagePrefab;
    }

    void Start()
    {
        GenerateInventoryUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DeselectItem();
        }
    }

    // Generate inventory UI and assign correct indexes
    public void GenerateInventoryUI()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            InventoryItem item = inventoryItems[i];
            item.index = i;

            Transform slot = inventoryPanel.GetChild(i);
            if (slot.childCount == 0) // Ensure only one item per slot
            {
                GameObject uiElement = Instantiate(item.itemImagePrefab, slot);

                ItemSelector itemSelector = uiElement.AddComponent<ItemSelector>();
                itemSelector.itemIndex = item.index;
                itemSelector.inventoryManager = this;

                Button button = uiElement.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.RemoveAllListeners(); // Prevent multiple listeners
                    button.onClick.AddListener(() => SelectItemByIndex(item.index));
                }

                Debug.Log($"UI Assigned: {item.itemName} (Index: {item.index})");
            }
        }
    }

    // Select an item using the inventory index
    public void SelectItemByIndex(int index)
    {
        Debug.Log($"Selecting item at index: {index}");

        // Destroy the previous item properly
        if (currentItem != null)
        {
            Destroy(currentItem);
        }

        if (index >= 0 && index < inventoryItems.Count)
        {
            InventoryItem selectedItem = inventoryItems[index];

            if (selectedItem.itemPrefab != null)
            {
                Debug.Log($"Spawning item: {selectedItem.itemPrefab.name}");

                currentItem = Instantiate(selectedItem.itemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
                currentItem.transform.parent = itemSpawnPoint;

                // ✅ If selecting index 0, manually enable/disable items
                if (index == 1)
                {
                    ToggleSpecificItem("Shovel(Clone)", false);
                    ToggleSpecificItem("Pokedex(Clone)", true);
                }

                ToggleHandMeshes(false);
            }
        }
        else
        {
            Debug.LogError($"Invalid item index: {index}");
        }
    }

    // ✅ Function to enable/disable specific spawned objects
    private void ToggleSpecificItem(string itemName, bool state)
    {
        foreach (Transform child in itemSpawnPoint)
        {
            if (child.gameObject.name == itemName)
            {
                child.gameObject.SetActive(state);
            }
        }
    }

    // Deselect the current item
    public void DeselectItem()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
            ToggleHandMeshes(true);
        }
    }

    // Enable/Disable hand meshes
    public void ToggleHandMeshes(bool state)
    {
        if (handControllerParent == null) return;

        foreach (Transform child in handControllerParent)
        {
            child.gameObject.SetActive(state);
        }
    }
}
