using UnityEngine;

public class SnapManager : MonoBehaviour
{
    public GameObject snapPoint;       // Reference to the snap point GameObject
    public GameObject objectToEnable;  // Reference to the GameObject to enable after snapping
    public Collider[] collidersToDisableTrigger; // Array of colliders whose isTrigger should be set to false

    private bool isOccupied = false;   // Ensures only one plant can be snapped
    private GameObject snappedPlant;   // Store the snapped plant for tracking

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied) return; // Prevent multiple plants from snapping

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

            Debug.Log("Snapping " + objectName);

            // Move and rotate the object to snap point position
            if (snapPoint != null)
            {
                other.transform.position = snapPoint.transform.position;
                other.transform.rotation = snapPoint.transform.rotation;

                // Disable the snap point
                snapPoint.SetActive(false);
            }

            // Disable physics to "lock" it in place
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // ✅ Enable the referenced GameObject
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
                Debug.Log(objectToEnable.name + " has been enabled.");
            }

            // ✅ Set isTrigger = false for all assigned colliders
            if (collidersToDisableTrigger != null && collidersToDisableTrigger.Length > 0)
            {
                foreach (Collider col in collidersToDisableTrigger)
                {
                    if (col != null)
                    {
                        col.isTrigger = false;
                        Debug.Log("Collider " + col.gameObject.name + " isTrigger set to FALSE.");
                    }
                }
            }

            // Store the snapped plant and mark as occupied
            snappedPlant = other.gameObject;
            isOccupied = true; 

            Debug.Log("Plant " + snappedPlant.name + " is now locked in place.");
        }
        else
        {
            Debug.Log(objectName + " does not have the 'Plant' tag.");
        }
    }

    public GameObject GetSnappedPlant()
    {
        return snappedPlant; // Allows other scripts to access the snapped plant
    }
}
