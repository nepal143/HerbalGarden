using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.UI; // For UI elements

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

    public Image[] panelSlots; // Assign 4 panel UI images in Inspector
    
    public GameObject[] objectsToEnable; // Objects to enable on reset
    public GameObject[] objectsToDisable; // Objects to disable on reset
    public GameObject[] objectsWithTriggersToEnable; // Objects whose triggers should be enabled
    public GameObject[] objectsWithCollidersToDisable; // Objects whose colliders should be disabled

    public Transform spawnPoint; // Spawn point for matched medicine objects
    public GameObject[] medicinePrefabs; // Array of 4 prefabs to spawn on medicine match

    private void Start()
    {
        LoadMedicineData();
        UpdatePanelColors(); // Initialize panel colors
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

    public void AddPlant(GameObject plant)
    {
        if (storedPlants.Count < panelSlots.Length) // Ensure max 4 slots
        {
            if (!storedPlants.Contains(plant))
            {
                storedPlants.Add(plant);
                Debug.Log("Plant added: " + plant.name);
                UpdatePanelColors(); // Update UI panel colors
            }
            else
            {
                Debug.Log("Plant already exists in storage: " + plant.name);
            }
        }
        else
        {
            Debug.Log("Storage full! Can't add more plants.");
        }
    }

    private void UpdatePanelColors()
    {
        for (int i = 0; i < panelSlots.Length; i++)
        {
            if (i < storedPlants.Count)
            {
                panelSlots[i].color = new Color32(0x00, 0x11, 0xFF, 0xFF); // Hex: #0011FF
            }
            else
            {
                panelSlots[i].color = Color.white; // Default color (Change as needed)
            }
        }
    }

    public void CheckForMedicine()
    {
        if (medicineData == null)
        {
            Debug.LogError("Medicine data not loaded.");
            return;
        }

        List<string> collectedPlants = storedPlants.Select(p => p.name.Replace("(Clone)", "").Trim()).ToList();

        bool foundMedicine = false;
        string matchedMedicine = "";

        foreach (MedicineData medicine in medicineData.medicines)
        {
            if (medicine.ingredients.All(ingredient => collectedPlants.Contains(ingredient)))
            {
                Debug.Log("✅ Matching Medicine Found: " + medicine.medicine);
                foundMedicine = true;
                matchedMedicine = medicine.medicine;
                SpawnMedicine();
                break;
            }
        }

        Color32 newColor = foundMedicine ? new Color32(0x00, 0xFF, 0x1E, 0xFF) : new Color32(0xFF, 0x0F, 0x00, 0xFF);

        foreach (Image panel in panelSlots)
        {
            panel.color = newColor;
        }

        if (foundMedicine)
        {
            Debug.Log("📢 Medicine Matched: " + matchedMedicine);
        }
    }

    private void SpawnMedicine()
{
    if (medicinePrefabs.Length > 0 && spawnPoint != null)
    {
        int randomIndex = Random.Range(0, medicinePrefabs.Length);
        GameObject spawnedMedicine = Instantiate(medicinePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        // Ensure the spawned object has a valid tag based on the matched medicine
        if (spawnedMedicine != null)
        {
            string matchedMedicine = medicineData.medicines
                .FirstOrDefault(m => m.ingredients.All(ingredient => storedPlants.Select(p => p.name.Replace("(Clone)", "").Trim()).Contains(ingredient)))
                ?.medicine;

            if (!string.IsNullOrEmpty(matchedMedicine))
            {
                spawnedMedicine.tag = matchedMedicine;
                Debug.Log($"💊 Medicine Spawned: {matchedMedicine} at {spawnPoint.position} with tag '{matchedMedicine}'");
            }
            else
            {
                Debug.LogWarning("⚠ No matching medicine found to set the tag.");
            }
        }
    }
    else
    {
        Debug.LogError("❌ No prefabs assigned or spawn point missing!");
    }
}

    public void ResetGame()
    {
        foreach (GameObject plant in storedPlants)
        {
            if (plant != null)
            {
                Destroy(plant);
            }
        }
        storedPlants.Clear();

        foreach (Image panel in panelSlots)
        {
            panel.color = Color.white;
        }

        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null) obj.SetActive(true);
        }

        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null) obj.SetActive(false);
        }

        foreach (GameObject obj in objectsWithTriggersToEnable)
        {
            Collider collider = obj.GetComponent<Collider>();
            if (collider != null) collider.isTrigger = true;
        }

        foreach (GameObject obj in objectsWithCollidersToDisable)
        {
            if (obj != null)
            {
                Collider[] colliders = obj.GetComponents<Collider>();

                if (colliders.Length > 0)
                {
                    foreach (Collider col in colliders)
                    {
                        col.enabled = false;
                    }
                    Debug.Log("✅ Collider disabled for: " + obj.name);
                }
                else
                {
                    Debug.LogWarning("⚠ No collider found on: " + obj.name);
                }
            }
        }

        Debug.Log("🔄 Reset Complete: Objects deleted, panels reset, colliders updated.");
    }
}
