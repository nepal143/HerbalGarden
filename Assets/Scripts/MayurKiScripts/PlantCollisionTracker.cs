using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public GameObject detectedPlant; // Stores the detected plant

    public GameObject snapPoint; // Reference to the Snap Point GameObject
    public Collider[] collidersToEnable; // Array of colliders to set isTrigger = true
    public GameObject gameObjectToEnableOnRelease; // GameObject to enable when releasing plant

    private void Update()
    {
        // Check if detected plant was destroyed externally
        if (detectedPlant == null || detectedPlant.Equals(null)) 
        {
            detectedPlant = null; // Reset reference
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Allow detection every time a new plant is found
        if (other.CompareTag("Plant")) 
        {
            detectedPlant = other.gameObject;
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

        // ✅ Enable the assigned GameObject
        if (gameObjectToEnableOnRelease != null)
        {
            gameObjectToEnableOnRelease.SetActive(true);
            Debug.Log("Enabled GameObject: " + gameObjectToEnableOnRelease.name);

            // ✅ Also enable its collider if it has one
            Collider objCollider = gameObjectToEnableOnRelease.GetComponent<Collider>();
            if (objCollider != null)
            {
                objCollider.enabled = true;
                Debug.Log("Enabled Collider for: " + gameObjectToEnableOnRelease.name);
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
            detectedPlant = null; // ✅ Reset reference
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
