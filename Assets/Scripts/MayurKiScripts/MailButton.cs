using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;

[System.Serializable]
public class MailEntry
{
    public int id;
    public string subject;
    public string body;
}

[System.Serializable]
public class MailList
{
    public List<MailEntry> mails;
}

public class MailButton : MonoBehaviour
{
    public GameObject mailPanel;
    public TextMeshProUGUI mailText;
    public Button closeButton;
    public Image buttonImage;
    public RawImage mailDisplayImage; // Common RawImage for mail images
    public Sprite assignedSprite; // Unique sprite assigned to each button
    public Sprite highlightSprite; // New sprite when button turns green

    private bool isRead = false;
    private MailList mailData;
    public int mailID;
    private static MailButton lastOpenedMail; // Tracks the last opened mail button

    private Color targetColor = new Color(0f, 165f / 255f, 3f / 255f); // #00A503
    private bool isColorChanged = false;

    private void Start()
    {
        LoadMailData();
        if (mailPanel != null)
            mailPanel.SetActive(false);

        if (mailDisplayImage != null)
            mailDisplayImage.gameObject.SetActive(false); // Initially disable image

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMail);

        StartCoroutine(TrackButtonColor());
    }

    void LoadMailData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "mails.json");
        if (File.Exists(filePath))
        {
            try
            {
                string jsonText = File.ReadAllText(filePath);
                mailData = JsonUtility.FromJson<MailList>(jsonText);
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading JSON file: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Mail JSON file not found at: " + filePath);
        }
    }

    public void OpenMail()
    {
        if (lastOpenedMail != null && lastOpenedMail != this)
        {
            lastOpenedMail.HideMailImage();
        }

        lastOpenedMail = this; // Update the last opened mail

        if (mailPanel != null && mailText != null && mailData != null)
        {
            MailEntry mail = mailData.mails.Find(m => m.id == mailID);
            if (mail != null)
            {
                mailText.text = $"Subject: {mail.subject}\n\n{mail.body}";
                mailPanel.SetActive(true);
            }
            else
            {
                mailText.text = "No mail found.";
                mailPanel.SetActive(true);
            }
        }

        if (!isRead)
        {
            SetButtonTransparency(0.5f);
            isRead = true;
        }

        // Display the assigned sprite on the RawImage
        if (mailDisplayImage != null && assignedSprite != null)
        {
            mailDisplayImage.texture = assignedSprite.texture;
            mailDisplayImage.gameObject.SetActive(true); // Enable image
        }
    }

    private void CloseMail()
    {
        if (mailPanel != null)
            mailPanel.SetActive(false);
    }

    private void HideMailImage()
    {
        if (mailDisplayImage != null)
        {
            mailDisplayImage.gameObject.SetActive(false); // Hide the previous mail's image
        }
    }

    private void SetButtonTransparency(float alpha)
    {
        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = alpha;
            buttonImage.color = color;
        }
    }

    public void HighlightMail()
    {
        if (buttonImage != null)
        {
            buttonImage.color = targetColor; // Change button color to green
            Debug.Log("✅ Mail ID " + mailID + " highlighted as matched.");
        }
    }

    private IEnumerator TrackButtonColor()
    {
        while (true)
        {
            if (buttonImage != null)
            {
                // Check if the color is **close** to green
                if (!isColorChanged && ColorsAreSimilar(buttonImage.color, targetColor, 0.01f))
                {
                    Debug.Log("🎨 Button turned green! Updating sprite...");

                    // Change sprite instantly
                    if (highlightSprite != null)
                    {
                        assignedSprite = highlightSprite;

                        // Update the displayed sprite in UI
                        if (mailDisplayImage != null)
                        {
                            mailDisplayImage.texture = highlightSprite.texture;
                            mailDisplayImage.gameObject.SetActive(true);
                        }
                    }

                    isColorChanged = true; // Prevent unnecessary updates
                }
            }
            yield return null; // ✅ Check **every frame** for instant response
        }
    }

    // Helper function to compare colors with a small tolerance
    private bool ColorsAreSimilar(Color c1, Color c2, float tolerance)
    {
        return Mathf.Abs(c1.r - c2.r) < tolerance &&
               Mathf.Abs(c1.g - c2.g) < tolerance &&
               Mathf.Abs(c1.b - c2.b) < tolerance;
    }
}
