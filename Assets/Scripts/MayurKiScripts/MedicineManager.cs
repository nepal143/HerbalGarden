using UnityEngine;

public class MedicineManager : MonoBehaviour
{
    public GameObject detectedMedicine; // Stores the detected medicine

    public GameObject snapPoint; // Reference to the Snap Point GameObject
    public Collider[] collidersToEnable; // Array of colliders to set isTrigger = true
    public GameObject gameObjectToEnableOnRelease; // GameObject to enable when releasing medicine

    private string[] medicineTags = {
        "Chyawanprash", "Triphala Churna", "Ashwagandha Rasayana", "Kadha",
        "Brahmi Tonic", "Neem Oil", "Bael Syrup", "Musli Pak", "Turmeric Milk"
    };

    private void Update()
    {
        // Check if detected medicine was destroyed externally
        if (detectedMedicine == null || detectedMedicine.Equals(null)) 
        {
            detectedMedicine = null; // Reset reference
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Allow detection if the collided object is tagged as a medicine
        foreach (string tag in medicineTags)
        {
            if (other.CompareTag(tag))
            {
                detectedMedicine = other.gameObject;
                Debug.Log("Medicine detected and stored: " + detectedMedicine.name);
                return;
            }
        }
    }

    // Called when button is pressed to release the medicine
    public void ReleaseMedicine()
    {
        if (detectedMedicine != null)
        {
            Rigidbody rb = detectedMedicine.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                Debug.Log("Released medicine: " + detectedMedicine.name);
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

    // Called when another button is pressed to delete the medicine & reset scene elements
    public void DeleteMedicine()
    {
        if (detectedMedicine != null)
        {
            Debug.Log("Deleting medicine: " + detectedMedicine.name);
            Destroy(detectedMedicine);
            detectedMedicine = null; // ✅ Reset reference
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
