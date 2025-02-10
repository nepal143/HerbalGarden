using UnityEngine;
using System.Collections.Generic;

public class MedicineSnapManager : MonoBehaviour
{
    public GameObject snapPoint;       // Reference to the snap point GameObject
    public GameObject objectToEnable;  // Reference to the GameObject to enable after snapping
    public Collider[] collidersToDisableTrigger; // Array of colliders whose isTrigger should be set to false

    private bool isOccupied = false;   // Ensures only one medicine can be snapped
    private GameObject snappedMedicine;   // Store the snapped medicine for tracking

    // List of valid medicine tags
    private List<string> validMedicines = new List<string>
    {
        "Chyawanprash",
        "Triphala Churna",
        "Ashwagandha Rasayana",
        "Kadha",
        "Brahmi Tonic",
        "Neem Oil",
        "Bael Syrup",
        "Musli Pak",
        "Turmeric Milk"
    };

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied) return; // Prevent multiple medicines from snapping

        string objectTag = other.gameObject.tag;
        Debug.Log("Collision detected with: " + objectTag);

        if (validMedicines.Contains(objectTag))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && rb.isKinematic) 
            {
                Debug.Log("Medicine is still being held, waiting for release...");
                return; // Don't snap while the medicine is being held
            }

            Debug.Log("Snapping " + objectTag);

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

            // Store the snapped medicine and mark as occupied
            snappedMedicine = other.gameObject;
            isOccupied = true; 

            Debug.Log("Medicine " + snappedMedicine.name + " is now locked in place.");
        }
        else
        {
            Debug.Log(objectTag + " is not a recognized medicine.");
        }
    }

    public GameObject GetSnappedMedicine()
    {
        return snappedMedicine; // Allows other scripts to access the snapped medicine
    }
}
