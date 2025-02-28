using UnityEngine;

public class EnableUIOnZRotation : MonoBehaviour
{
    [Tooltip("The hand object whose Z-axis rotation will be monitored.")]
    public Transform handObject;

    [Tooltip("The UI GameObject to enable or disable.")]
    public GameObject uiElement;

    [Tooltip("Check to enable Clockwise detection, uncheck for Anti-Clockwise.")]
    public bool isClockwise = false; // Default: Anti-Clockwise

    private void Start()
    {
        if (uiElement != null)
            uiElement.SetActive(false); // Ensure UI is hidden at start
    }

    private void Update()
    {
        if (handObject == null || uiElement == null)
        {
            Debug.LogWarning("Hand object or UI element not assigned.");
            return;
        }

        // Get the hand object's local z-axis rotation
        float handRotationZ = handObject.localEulerAngles.z;

        // Adjust for Unity's 360-degree rotation wrapping
        if (handRotationZ > 180)
            handRotationZ -= 360;

        // Enable or disable UI based on rotation direction
        if (isClockwise)
        {
            // UI appears when rotating clockwise between 60° and 180°
            uiElement.SetActive(handRotationZ >= 60 && handRotationZ <= 180);
        }
        else
        {
            // UI appears when rotating counterclockwise between -60° and -180°
            uiElement.SetActive(handRotationZ <= -60 && handRotationZ >= -180);
        }
    }
}
