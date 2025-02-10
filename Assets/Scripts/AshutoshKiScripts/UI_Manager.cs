// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class UI_Manager : MonoBehaviour
// {
//     public TMP_Text nameText;
//     public TMP_Text scientificNameText;
//     public TMP_Text usesText;
//     public TMP_Text benefitsText;

//     public void UpdatePlantDetails(Plant plant)
//     {
//         // Populate UI elements with plant details
//         nameText.text = $"Name: {plant.name}";
//         scientificNameText.text = $"Scientific Name: {plant.scientificName}";
//         usesText.text = $"Uses: {plant.uses}";
//         benefitsText.text = $"Benefits: {plant.benefits}";
//     }

//     public void ClearDetails()
//     {
//         // Clear UI when no plant is detected
//         nameText.text = "";
//         scientificNameText.text = "";
//         usesText.text = "";
//         benefitsText.text = "";
//     }
// }


using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text scientificNameText;
    public TMP_Text usesText;
    public TMP_Text benefitsText;

    public void UpdatePlantDetails(Plant plant)
    {
        if (plant != null)
        {
            nameText.text = $"🌿 Name: {plant.name}";
            scientificNameText.text = $"🔬 Scientific Name: {plant.scientificName}";
            usesText.text = $"💊 Uses: {plant.uses}";
            benefitsText.text = $"✨ Benefits: {plant.benefits}";
        }
        else
        {
            ClearDetails();
        }
    }

    public void ClearDetails()
    {
        nameText.text = "";
        scientificNameText.text = "";
        usesText.text = "";
        benefitsText.text = "";
    }
}

