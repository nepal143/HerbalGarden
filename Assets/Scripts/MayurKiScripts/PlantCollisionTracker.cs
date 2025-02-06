using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public GameObject detectedPlant; // Stores the detected plant
    private bool hasDetectedPlant = false; // Ensures only one plant is assigned

    public GameObject snapPoint; // Reference to the Snap Point GameObject
    public Collider[] collidersToEnable; // Array of colliders to set isTrigger = true

    private void OnTriggerEnter(Collider other)
    {
        if (hasDetectedPlant) return; // Ignore further detections

        if (other.CompareTag("Plant"))
        {
            detectedPlant = other.gameObject;
            hasDetectedPlant = true;
            Debug.Log("Plant detected and stored: " + detectedPlant.name);
        }
    }

    // Called when button is pressed to release the plant
    public void ReleasePlant()
    {
        if (detectedPlant != null)
        {
            Rigidbody rb = detectedPlant.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                Debug.Log("Released plant: " + detectedPlant.name);
            }
        }
    }

    // Called when another button is pressed to delete the plant & reset scene elements
    public void DeletePlant()
    {
        if (detectedPlant != null)
        {
            Debug.Log("Deleting plant: " + detectedPlant.name);
            Destroy(detectedPlant);
            detectedPlant = null; // Reset reference
        }

        // Enable snap point if assigned
        if (snapPoint != null)
        {
            snapPoint.SetActive(true);
            Debug.Log("Snap point enabled.");
        }

        // Enable isTrigger for all specified colliders
        foreach (Collider col in collidersToEnable)
        {
            if (col != null)
            {
                col.isTrigger = true;
                Debug.Log("Enabled isTrigger for: " + col.gameObject.name);
            }
        }

        // Disable this GameObject
        Debug.Log(gameObject.name + " is now disabled.");
        gameObject.SetActive(false);
    }
}
