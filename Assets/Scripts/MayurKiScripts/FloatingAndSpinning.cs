using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatMagnitude = 0.5f; // How high the object moves up and down
    public float floatSpeed = 1.0f; // Speed of floating motion

    [Header("VR Gaze Interaction")]
    public Transform vrController; // Assign your VR controller or camera's transform
    public float gazeDistance = 10f; // Max distance for detecting the cube
    public float multiplyScale = 1.1f;

    private Vector3 startPosition;
    private Vector3 originalScale;
    private bool isScaled = false; // Tracks if the object is currently scaled

    void Start()
    {
        startPosition = transform.position;
        originalScale = transform.localScale; // Store the original size
    }

    void Update()
    {
        // Floating Effect (Sin wave motion)
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatMagnitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Check if the VR controller is pointing at this object
        bool isLookingAtObject = false;

        if (vrController != null)
        {
            Ray ray = new Ray(vrController.position, vrController.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, gazeDistance))
            {
                if (hit.transform == transform) // If this object is hit
                {
                    isLookingAtObject = true;
                }
            }
        }

        // Scale up when pointing, shrink back when not pointing
        if (isLookingAtObject && !isScaled)
        {
            transform.localScale = originalScale * multiplyScale; // Scale up
            isScaled = true;
        }
        else if (!isLookingAtObject && isScaled)
        {
            transform.localScale = originalScale; // Shrink back
            isScaled = false;
        }
    }
}
