using UnityEngine;

public class SnapToPoint : MonoBehaviour
{
    [Header("Snap Settings")]
    public Transform snapPoint; // Assign this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the tag "Plant"
        if (other.CompareTag("Plant"))
        {
            Debug.Log("collision ho rha hai");
            // Snap the object to the snap point
            other.transform.position = snapPoint.position;
            other.transform.rotation = snapPoint.rotation;
        }
    }
}
