using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class PlantData // Renamed to avoid conflict
{
    public int id;
    public string name;
    public string description;
    public string environment;
    public string medicinalUses;
}

[System.Serializable]
public class PlantDataCollection // Renamed to avoid conflict
{
    public List<PlantData> plants;
}

public class PlantInfoManager : MonoBehaviour
{
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;

    private PlantDataCollection plantDataCollection; // Updated variable name
    private int activePlantID = -1;

    public int plantID; // Assign a unique ID to each button in the Inspector

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
                infoText.text = $"<b>{plant.name}</b>\n\n{plant.description}\n\n<b>Environment:</b> {plant.environment}\n\n<b>Medicinal Uses:</b> {plant.medicinalUses}";
                infoPanel.SetActive(true);
                activePlantID = plantID;
            }
            else
            {
                infoText.text = "No plant info found.";
                infoPanel.SetActive(true);
            }
        }
    }
}
