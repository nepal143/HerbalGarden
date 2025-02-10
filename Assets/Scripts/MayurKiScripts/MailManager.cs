using UnityEngine;
using UnityEngine.UI;

public class MailManager : MonoBehaviour
{
    public GameObject[] mailObjects; // Array of mail GameObjects (ordered by Mail ID)
    public MedicineManager medicineManager; // Reference to MedicineManager

    private void Start()
    {
        if (medicineManager == null)
        {
            Debug.LogError("❌ MedicineManager reference is missing!");
        }
    }

    // Call this function after medicine is matched
    public void HighlightMatchedMail(int matchedMailID)
    {
        foreach (GameObject mailObj in mailObjects)
        {
            MailButton mailButton = mailObj.GetComponent<MailButton>(); // 🔥 Updated to MailButton
            if (mailButton != null && mailButton.mailID == matchedMailID)
            {
                ChangeMailColor(mailObj);
                Debug.Log("✅ Mail ID " + matchedMailID + " matched & highlighted!");
                return;
            }
        }

        Debug.Log("⚠️ No mail found with ID: " + matchedMailID);
    }

    // Changes the color of matched mail
    private void ChangeMailColor(GameObject mailObj)
    {
        Image mailImage = mailObj.GetComponent<Image>(); // Assuming mails have UI Image component
        if (mailImage != null)
        {
            mailImage.color = new Color(0f, 0.647f, 0.02f); // RGB for #00A503
        }
        else
        {
            Debug.LogError("❌ No Image component found on " + mailObj.name);
        }
    }
}
