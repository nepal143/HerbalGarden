using UnityEngine;

public class EnableUIOnZRotation : MonoBehaviour
{
    [Tooltip("The hand object whose Z-axis rotation will be monitored.")]
    public Transform handObject;

    [Tooltip("The UI GameObject to enable or disable.")]
    public GameObject uiElement;

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

        // Enable or disable the UI based on the Z rotation range
        if (handRotationZ <= -60 && handRotationZ >= -180)
        {
            uiElement.SetActive(true); // Enable the UI
        }
        else
        {
            uiElement.SetActive(false); // Disable the UI
        }
    }
}
