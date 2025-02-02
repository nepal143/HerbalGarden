using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MailSystem : MonoBehaviour
{
    public GameObject mailPanel; // Reference to the mail panel
    public TextMeshProUGUI mailText; // Reference to the text inside the panel
    public Button closeButton; // Reference to the close button
    public Image buttonImage; // Reference to the button's image (for transparency change)

    [TextArea(3, 10)]
    public string mailContent; // Mail content for this button

    private bool isRead = false; // Track if the mail has been read

    private void Start()
    {
        if (mailPanel != null)
            mailPanel.SetActive(false); // Keep panel hidden initially

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMail);
    }

    public void OpenMail()
    {
        if (mailPanel != null && mailText != null)
        {
            mailText.text = mailContent; // Update mail content
            mailPanel.SetActive(true); // Show panel
        }

        if (!isRead) // Only change transparency the first time
        {
            SetButtonTransparency(0.5f);
            isRead = true;
        }
    }

    private void CloseMail()
    {
        if (mailPanel != null)
            mailPanel.SetActive(false); // Hide panel
    }

    private void SetButtonTransparency(float alpha)
    {
        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = alpha; // Set new transparency
            buttonImage.color = color;
        }
    }
}
