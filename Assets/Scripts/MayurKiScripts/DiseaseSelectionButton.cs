using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[System.Serializable]
public class DiseaseMedicineData
{
    public List<string> diseases;
    public string medicine;
    public List<string> ingredients;
}

[System.Serializable]
public class DiseaseMedicineList
{
    public List<DiseaseMedicineData> data;
}

public class DiseaseSelectionButton : MonoBehaviour
{
    public TMP_Text logPanelText; // Assign TextMeshPro UI for selected diseases
    public TMP_Text resultPanelText; // Assign TextMeshPro UI for displaying medicine results
    public Button submitButton; // Submit button reference

    private Button button;
    private static List<string> selectedDiseases = new List<string>(); // Stores selected diseases
    private static int maxSelections = 5; // Limit to 5 diseases
    private static List<DiseaseMedicineData> diseaseMedicineList = new List<DiseaseMedicineData>(); // Stores JSON data

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(ToggleDiseaseSelection);
        }

        // Load JSON data once
        if (diseaseMedicineList.Count == 0)
        {
            LoadData();
        }

        if (submitButton != null)
        {
            submitButton.onClick.AddListener(CheckForMedicineMatch);
        }
    }

    void LoadData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "disease_medicine_data.json");
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            DiseaseMedicineList dataList = JsonUtility.FromJson<DiseaseMedicineList>("{\"data\":" + jsonData + "}");
            diseaseMedicineList = dataList.data;

            Debug.Log("JSON file successfully loaded!"); 
            foreach (var entry in diseaseMedicineList)
            {
                Debug.Log($"Diseases: {string.Join(", ", entry.diseases)} | Medicine: {entry.medicine} | Ingredients: {string.Join(", ", entry.ingredients)}");
            }
        }
        else
        {
            Debug.LogError("JSON file not found!");
        }
    }

    void ToggleDiseaseSelection()
    {
        string diseaseName = button.GetComponentInChildren<TMP_Text>().text.Trim();

        if (selectedDiseases.Contains(diseaseName))
        {
            selectedDiseases.Remove(diseaseName); // Deselect if already selected
        }
        else
        {
            if (selectedDiseases.Count < maxSelections)
            {
                selectedDiseases.Add(diseaseName);
            }
        }

        UpdateLogPanel();
    }

    void UpdateLogPanel()
    {
        logPanelText.text = "Selected Diseases:\n" + string.Join("\n", selectedDiseases);
    }

    void CheckForMedicineMatch()
    {
        HashSet<string> selectedSet = new HashSet<string>(selectedDiseases.Select(d => d.ToLower().Trim())); // Normalize user input

        foreach (var entry in diseaseMedicineList)
        {
            HashSet<string> jsonSet = new HashSet<string>(entry.diseases.Select(d => d.ToLower().Trim())); // Normalize JSON data

            if (jsonSet.SetEquals(selectedSet))
            {
                resultPanelText.text = "Medicine: " + entry.medicine + "\nIngredients: " + string.Join(", ", entry.ingredients);
                return;
            }
        }
        resultPanelText.text = "No matching medicine found.";
    }
}
