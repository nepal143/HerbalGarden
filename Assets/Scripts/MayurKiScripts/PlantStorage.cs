using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

[System.Serializable]
public class MedicineData
{
    public List<string> diseases;
    public string medicine;
    public List<string> ingredients;
}

[System.Serializable]
public class MedicineList
{
    public List<MedicineData> medicines;
}

public class PlantStorage : MonoBehaviour
{
    public List<GameObject> storedPlants = new List<GameObject>(); // List of stored plants
    private MedicineList medicineData; // Stores parsed JSON data

    private void Start()
    {
        LoadMedicineData();
    }

    private void LoadMedicineData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "disease_medicine_data.json");
        Debug.Log("Looking for JSON at: " + filePath); // 🔍 Debugging

        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            medicineData = JsonUtility.FromJson<MedicineList>("{\"medicines\":" + jsonText + "}");
        }
        else
        {
            Debug.LogError("❌ JSON file not found: " + filePath);
        }
    }


    // ✅ Function to add a plant to storage
    public void AddPlant(GameObject plant)
    {
        if (!storedPlants.Contains(plant))
        {
            storedPlants.Add(plant);
            Debug.Log("Plant added: " + plant.name);
        }
        else
        {
            Debug.Log("Plant already exists in storage: " + plant.name);
        }
    }

    // ✅ Function to check for matching medicine based on collected plants
    public void CheckForMedicine()
    {
        if (medicineData == null)
        {
            Debug.LogError("Medicine data not loaded.");
            return;
        }

        List<string> collectedPlants = storedPlants.Select(p => p.name.Replace("(Clone)", "").Trim()).ToList();

        foreach (MedicineData medicine in medicineData.medicines)
        {
            if (medicine.ingredients.All(ingredient => collectedPlants.Contains(ingredient)))
            {
                Debug.Log("Matching Medicine Found: " + medicine.medicine);
                return;
            }
        }

        Debug.Log("No matching medicine found.");
    }
}
