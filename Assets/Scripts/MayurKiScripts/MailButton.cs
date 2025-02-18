using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
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
    public RawImage mailDisplayImage; // General RawImage to display mail sprite
    public Sprite assignedSprite; // Unique sprite assigned to each button

    private bool isRead = false;
    private MailList mailData;
    public int mailID;

    private void Start()
    {
        LoadMailData();
        if (mailPanel != null)
            mailPanel.SetActive(false);
        
        if (mailDisplayImage != null)
            mailDisplayImage.gameObject.SetActive(false); // Initially disable image

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMail);
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

        if (mailDisplayImage != null)
            mailDisplayImage.gameObject.SetActive(false); // Hide image on close
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
            buttonImage.color = new Color(0f, 0.647f, 0.02f); // RGB for #00A503
            Debug.Log("✅ Mail ID " + mailID + " highlighted as matched.");
        }
    }
}
