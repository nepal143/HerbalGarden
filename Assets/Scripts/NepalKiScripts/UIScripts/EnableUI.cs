using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Reference to the UI element (e.g., panel, button, etc.)
    public GameObject uiElement;

    // Enable the UI element
    public void EnableUI()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }
        else
        {
            Debug.LogWarning("UI Element is not assigned in the inspector.");
        }
    }

    // Disable the UI element
    public void DisableUI()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }
        else
        {
            Debug.LogWarning("UI Element is not assigned in the inspector.");
        }
    }

    // Toggle the UI element (Enable if disabled, Disable if enabled)
    public void ToggleUI()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(!uiElement.activeSelf);
        }
        else
        {
            Debug.LogWarning("UI Element is not assigned in the inspector.");
        }
    }
}
