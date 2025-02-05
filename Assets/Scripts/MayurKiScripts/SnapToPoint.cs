using UnityEngine;

public class SnapToPoint : MonoBehaviour
{
    public Transform snapPoint; // Assign this in the Inspector
    private bool isOccupied = false; // Prevent multiple plants from snapping

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied) return; // Prevent multiple snaps

        // Get the name of the collided object without "(Clone)"
        string objectName = other.gameObject.name.Replace("(Clone)", "").Trim();
        Debug.Log("Collision detected with: " + objectName);

        if (other.CompareTag("Plant"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && rb.isKinematic) 
            {
                Debug.Log("Plant is still being held, waiting for release...");
                return; // Don't snap while the plant is being held
            }

            Debug.Log("Snapping " + objectName + " to " + snapPoint.position);

            // Move and rotate the object to snap point
            other.transform.position = snapPoint.position;
            other.transform.rotation = snapPoint.rotation;

            // Disable physics to "lock" it in place
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // ✅ Disable the snap point to prevent further snapping **only after release**
            if (snapPoint != null)
            {
                snapPoint.gameObject.SetActive(false);
            }

            isOccupied = true; // Mark as occupied to prevent multiple snaps
        }
        else
        {
            Debug.Log(objectName + " does not have the 'Plant' tag.");
        }
    }
}
