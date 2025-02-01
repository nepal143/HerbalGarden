using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DiseaseSelectionButton : MonoBehaviour
{
    public TMP_Text logPanelText; // Assign the TextMeshPro UI component
    private Button button;
    private static List<string> selectedDiseases = new List<string>(); // Stores selected diseases
    private static int maxSelections = 5; // Limit to 5 diseases

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(ToggleDiseaseSelection);
        }
    }

    void ToggleDiseaseSelection()
    {
        string diseaseName = button.GetComponentInChildren<TMP_Text>().text;

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
}
