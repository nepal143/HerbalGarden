using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class PlantData
{
    public int id;
    public string name;
    public string description;
    public string habitat;  
    public string medicinalUses;
}

[System.Serializable]
public class PlantDataCollection
{
    public List<PlantData> plants;
}

public class PlantInfoManager : MonoBehaviour
{
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;
    public RawImage plantRawImage; // Raw Image component reference

    private PlantDataCollection plantDataCollection;
    private int activePlantID = -1;

    public int plantID;
    public Texture2D plantTexture; // Assign the plant texture manually in the editor

    private void Start()
    {
        LoadPlantData();
        if (infoPanel != null)
            infoPanel.SetActive(false); // Hide panel initially
    }

    void LoadPlantData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "PlantsInfo.json");

        if (File.Exists(filePath))
        {
            try
            {
                string jsonText = File.ReadAllText(filePath);
                plantDataCollection = JsonUtility.FromJson<PlantDataCollection>(jsonText);
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading JSON file: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Plant JSON file not found at: " + filePath);
        }
    }

    public void ShowPlantInfo()
    {
        if (activePlantID == plantID)
        {
            infoPanel.SetActive(false);
            activePlantID = -1;
            return;
        }

        if (infoPanel != null && infoText != null && plantDataCollection != null)
        {
            PlantData plant = plantDataCollection.plants.Find(p => p.id == plantID);
            if (plant != null)
            {
                infoText.text = $"<b>{plant.name}</b>\n\n<b>Description:</b> {plant.description}\n\n" +
                                $"<b>Habitat:</b> {plant.habitat}\n\n<b>Medicinal Uses:</b> {plant.medicinalUses}";

                infoPanel.SetActive(true);
                activePlantID = plantID;

                // Set the assigned texture
                if (plantRawImage != null && plantTexture != null)
                {
                    plantRawImage.texture = plantTexture; // Set the texture assigned in the editor
                    plantRawImage.gameObject.SetActive(true);
                }
            }
            else
            {
                infoText.text = "No plant info found.";
                infoPanel.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Info panel, Raw Image, or text component is not assigned.");
        }
    }
}
