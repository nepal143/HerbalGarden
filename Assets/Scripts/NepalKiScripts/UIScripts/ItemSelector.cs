using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSelector : MonoBehaviour, IPointerDownHandler
{
    public int itemIndex;
    public VRInventoryManager inventoryManager;

    void Start()
    {
        if (inventoryManager == null)
        {
            inventoryManager = GameObject.FindGameObjectWithTag("Inventory")?.GetComponent<VRInventoryManager>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryManager != null)
        {
            Debug.Log($"Item selected (Index: {itemIndex})");
            inventoryManager.SelectItemByIndex(itemIndex);
        }
    }
}
