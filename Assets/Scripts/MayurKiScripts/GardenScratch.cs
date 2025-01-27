using UnityEngine;

public class ActivatePanelWithCounter : MonoBehaviour
{
    [Header("References")]
    [Tooltip("First GameObject to monitor for collision.")]
    public GameObject objectA;

    [Tooltip("Second GameObject to monitor for collision.")]
    public GameObject objectB;

    [Tooltip("The panel to activate and deactivate.")]
    public GameObject panel;

    [Header("Settings")]
    [Tooltip("The distance threshold to toggle the panel.")]
    public float activationThreshold = 0.03f;

    private int pressCounter = 0;
    private bool isWithinRange = false;

    private void Start()
    {
        // Ensure the panel is initially inactive
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    private void Update()
    {
        // Check if both objects are assigned
        if (objectA == null || objectB == null || panel == null)
        {
            Debug.LogError("Please assign Object A, Object B, and the panel in the inspector.");
            return;
        }

        // Calculate the distance between the two objects
        float distance = Vector3.Distance(objectA.transform.position, objectB.transform.position);

        // Check if the distance is within the activation threshold
        if (distance < activationThreshold)
        {
            if (!isWithinRange) // Only trigger when entering the range
            {
                pressCounter++;
                TogglePanel();
                isWithinRange = true;
            }
        }
        else
        {
            // Reset the range flag when the objects move apart
            isWithinRange = false;
        }
    }

    private void TogglePanel()
    {
        // Activate or deactivate the panel based on the counter
        bool shouldActivate = pressCounter % 2 != 0; // Odd = activate, Even = deactivate
        panel.SetActive(shouldActivate);
    }
}
