using UnityEngine;
using System.IO;
using System.Collections;

public class MedicineManager : MonoBehaviour
{
    public GameObject detectedMedicine; // Stores the detected medicine

    public GameObject snapPoint; // Reference to the Snap Point GameObject
    public Collider[] collidersToEnable; // Array of colliders to set isTrigger = true
    public GameObject gameObjectToEnableOnRelease; // GameObject to enable when releasing medicine

    private string jsonPath;
    private string[] medicineTags = {
        "Chyawanprash", "Triphala Churna", "Ashwagandha Rasayana", "Kadha",
        "Brahmi Tonic", "Neem Oil", "Bael Syrup", "Musli Pak", "Turmeric Milk"
    };

    private void Start()
    {
        jsonPath = Path.Combine(Application.streamingAssetsPath, "mails.json");
    }

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
                Debug.Log("✅ Medicine detected: " + detectedMedicine.name);
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
                
                // ✅ Apply upward force
                rb.AddForce(Vector3.up * 50f, ForceMode.Impulse); 

                Debug.Log("🛠️ Medicine released: " + detectedMedicine.name);

                // ✅ Destroy after 1.5s
                StartCoroutine(DestroyMedicineAfterDelay(detectedMedicine, 1.5f));
            }

            // ✅ Check mails.json and log the corresponding mail ID
            CheckMailForMedicine(detectedMedicine.tag);
        }

        // ✅ Enable the assigned GameObject
        if (gameObjectToEnableOnRelease != null)
        {
            gameObjectToEnableOnRelease.SetActive(true);
            Debug.Log("✅ Enabled GameObject: " + gameObjectToEnableOnRelease.name);

            // ✅ Also enable its collider if it has one
            Collider objCollider = gameObjectToEnableOnRelease.GetComponent<Collider>();
            if (objCollider != null)
            {
                objCollider.enabled = true;
                Debug.Log("✅ Enabled Collider for: " + gameObjectToEnableOnRelease.name);
            }
        }
    }

    // Coroutine to destroy the medicine after a delay
    private IEnumerator DestroyMedicineAfterDelay(GameObject medicine, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (medicine != null)
        {
            Debug.Log("🔥 Medicine destroyed: " + medicine.name);
            Destroy(medicine);
        }
    }

    private void CheckMailForMedicine(string medicineTag)
{
    if (!File.Exists(jsonPath))
    {
        Debug.LogError("❌ mails.json not found at: " + jsonPath);
        return;
    }

    string jsonData = File.ReadAllText(jsonPath);
    MailData mailData = JsonUtility.FromJson<MailData>(jsonData);

    if (mailData == null || mailData.mails == null)
    {
        Debug.LogError("❌ Error loading mail data! File might be empty or incorrectly formatted.");
        return;
    }

    bool found = false;
    foreach (var mail in mailData.mails)
    {
        if (mail.medicine.name == medicineTag)
        {
            Debug.Log("✅ Medicine Match Found! Mail ID: " + mail.id + " | Medicine: " + medicineTag);
            found = true;

            // Find and highlight the mail in UI
            MailButton[] mailButtons = FindObjectsOfType<MailButton>();
            foreach (MailButton mailButton in mailButtons)
            {
                if (mailButton.mailID == mail.id)
                {
                    mailButton.HighlightMail();
                    break;
                }
            }
            break; // Stop checking after finding the first match
        }
    }

    if (!found)
    {
        Debug.Log("⚠️ No matching mail found for medicine: " + medicineTag);
    }
}


    // Called when another button is pressed to delete the medicine & reset scene elements
    public void DeleteMedicine()
    {
        if (detectedMedicine != null)
        {
            Debug.Log("🗑️ Deleting medicine: " + detectedMedicine.name);
            Destroy(detectedMedicine);
            detectedMedicine = null; // ✅ Reset reference
        }

        // Enable snap point if assigned
        if (snapPoint != null)
        {
            snapPoint.SetActive(true);
            Debug.Log("📌 Snap point enabled.");
        }

        // Enable isTrigger for all specified colliders
        foreach (Collider col in collidersToEnable)
        {
            if (col != null)
            {
                col.isTrigger = true;
                Debug.Log("🔄 Enabled isTrigger for: " + col.gameObject.name);
            }
        }

        // Disable this GameObject
        Debug.Log("🚫 " + gameObject.name + " is now disabled.");
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class MailData
{
    public Mail[] mails;
}

[System.Serializable]
public class Mail
{
    public int id;
    public string subject;
    public string body;
    public Medicine medicine;
}

[System.Serializable]
public class Medicine
{
    public string name;
    public string[] ingredients;
}
