using UnityEngine;
using TMPro;
using System.Collections;

public class UI_Manager : MonoBehaviour
{
    private TMP_Text nameText;
    private TMP_Text scientificNameText;
    private TMP_Text usesText;
    private TMP_Text benefitsText;
    private bool isUIReady = false;

    void Start()
    {
        StartCoroutine(FindUIElements());
    }

    private IEnumerator FindUIElements()
    {
        while (!isUIReady)
        {
            GameObject nameObj = GameObject.FindGameObjectWithTag("PlantNameText");
            GameObject scientificObj = GameObject.FindGameObjectWithTag("ScientificNameText");
            GameObject usesObj = GameObject.FindGameObjectWithTag("UsesText");
            GameObject benefitsObj = GameObject.FindGameObjectWithTag("BenefitsText");

            if (nameObj && scientificObj && usesObj && benefitsObj)
            {
                nameText = nameObj.GetComponent<TMP_Text>();
                scientificNameText = scientificObj.GetComponent<TMP_Text>();
                usesText = usesObj.GetComponent<TMP_Text>();
                benefitsText = benefitsObj.GetComponent<TMP_Text>();
                isUIReady = true;
            }
            
            yield return new WaitForSeconds(0.5f); // Check again after a short delay
        }
    }

    public void UpdatePlantDetails(Plant plant)
    {
        if (!isUIReady) return;

        if (plant != null)
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
        if (!isUIReady) return;

        nameText.text = "";
        scientificNameText.text = "";
        usesText.text = "";
        benefitsText.text = "";
    }
}
