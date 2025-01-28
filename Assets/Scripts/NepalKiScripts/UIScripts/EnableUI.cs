using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Enable the specified UI element
    public void EnableUI(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }
        else
        {
            Debug.LogWarning("UI Element is null. Make sure you pass a valid GameObject.");
        }
    }

    // Disable the specified UI element
    public void DisableUI(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }
        else
        {
            Debug.LogWarning("UI Element is null. Make sure you pass a valid GameObject.");
        }
    }

    // Toggle the specified UI element (Enable if disabled, Disable if enabled)
    public void ToggleUI(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(!uiElement.activeSelf);
        }
        else
        {
            Debug.LogWarning("UI Element is null. Make sure you pass a valid GameObject.");
        }
    }
}
