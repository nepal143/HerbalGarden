using UnityEngine;
using UnityEngine.UI;

public class BaseplateCollision : MonoBehaviour
{
    public GameObject detectedPlant; // Stores the detected plant
    public GameObject objectToDisable; // Assign in the Inspector (to disable on collision)
    public Canvas canvasToEnable; // Assign in the Inspector (to enable on collision)
    public PlantStorage plantStorage; // Reference to the storage system
    public Button storePlantButton; // Button to store the plant

    private void OnCollisionEnter(Collision collision)
    {
        if (detectedPlant == null && collision.gameObject.CompareTag("Plant"))
        {
            detectedPlant = collision.gameObject;

            // Get the name without "(Clone)"
            string plantName = detectedPlant.name.Replace("(Clone)", "").Trim();
            Debug.Log("Plant landed on baseplate: " + plantName);

            // Disable the referenced GameObject
            if (objectToDisable != null)
            {
                objectToDisable.SetActive(false);
                Debug.Log("Disabled: " + objectToDisable.name);
            }

            // Enable the referenced Canvas
            if (canvasToEnable != null)
            {
                canvasToEnable.gameObject.SetActive(true);
                Debug.Log("Enabled Canvas: " + canvasToEnable.name);
            }

            // Assign the button click event
            if (storePlantButton != null)
            {
                storePlantButton.onClick.RemoveAllListeners(); // Remove old events
                storePlantButton.onClick.AddListener(StorePlant);
                Debug.Log("Button set up to store the plant.");
            }
        }
    }

    // Function called when the button is pressed
    private void StorePlant()
    {
        if (detectedPlant != null && plantStorage != null)
        {
            plantStorage.AddPlant(detectedPlant);
            Debug.Log("Stored plant: " + detectedPlant.name);
        }
    }
}
