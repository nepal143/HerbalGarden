using UnityEngine;
using TMPro;
using System.Collections;

public class UI_Manager : MonoBehaviour
{
    private TMP_Text nameText;
    private TMP_Text scientificNameText;
    private TMP_Text usesText;
    private TMP_Text benefitsText;

    void Start()
    {
        StartCoroutine(FindUIElements()); // Start searching for UI elements
    }

    private IEnumerator FindUIElements()
    {
        while (true) // Keep checking in case UI is dynamically changed
        {
            GameObject nameObj = GameObject.FindGameObjectWithTag("PlantNameText");
            GameObject scientificObj = GameObject.FindGameObjectWithTag("ScientificNameText");
            GameObject usesObj = GameObject.FindGameObjectWithTag("UsesText");
            GameObject benefitsObj = GameObject.FindGameObjectWithTag("BenefitsText");

            nameText = nameObj ? nameObj.GetComponent<TMP_Text>() : null;
            scientificNameText = scientificObj ? scientificObj.GetComponent<TMP_Text>() : null;
            usesText = usesObj ? usesObj.GetComponent<TMP_Text>() : null;
            benefitsText = benefitsObj ? benefitsObj.GetComponent<TMP_Text>() : null;

            yield return new WaitForSeconds(0.5f); // Check again after a short delay
        }
    }

    public void UpdatePlantDetails(Plant plant)
    {
        // Make sure UI references are up to date before setting values
        RefreshUIReferences();

        if (plant != null && nameText && scientificNameText && usesText && benefitsText)
        {
            nameText.text = $"\U0001F33F Name: {plant.name}";
            scientificNameText.text = $"\U0001F52C Scientific Name: {plant.scientificName}";
            usesText.text = $"\U0001F48A Uses: {plant.uses}";
            benefitsText.text = $"\u2728 Benefits: {plant.benefits}";
        }
        else
        {
            ClearDetails();
        }
    }

    public void ClearDetails()
    {
        // Make sure UI references are up to date before clearing values
        RefreshUIReferences();

        if (nameText) nameText.text = "";
        if (scientificNameText) scientificNameText.text = "";
        if (usesText) usesText.text = "";
        if (benefitsText) benefitsText.text = "";
    }

    private void RefreshUIReferences()
    {
        if (!nameText || !scientificNameText || !usesText || !benefitsText)
        {
            GameObject nameObj = GameObject.FindGameObjectWithTag("PlantNameText");
            GameObject scientificObj = GameObject.FindGameObjectWithTag("ScientificNameText");
            GameObject usesObj = GameObject.FindGameObjectWithTag("UsesText");
            GameObject benefitsObj = GameObject.FindGameObjectWithTag("BenefitsText");

            nameText = nameObj ? nameObj.GetComponent<TMP_Text>() : null;
            scientificNameText = scientificObj ? scientificObj.GetComponent<TMP_Text>() : null;
            usesText = usesObj ? usesObj.GetComponent<TMP_Text>() : null;
            benefitsText = benefitsObj ? benefitsObj.GetComponent<TMP_Text>() : null;
        }
    }
}


// mere chain main sab ujdaaaa 
//  music :: dhunak dhunak dhundhun 
// jalim nazer hata le 
//  music :: dhunak dhunak dhundhun 
// mere chain main sab ujdaaaa 
// jalim nazer hata le 
// barbad ho gaye hai hum to  .. 
//  sorry I am not a singer written by nepal143 date 2/28/2025