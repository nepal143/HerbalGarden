// using UnityEngine;

// public class PlantRaycaster : MonoBehaviour
// {
//     public float rayDistance = 10f; // Adjust the distance based on your needs
//     public UI_Manager uiManager;

//     void Update()
//     {
//         // Shoot a raycast from the Pokedex camera
//         Ray ray = new Ray(transform.position, transform.forward);
//         if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
//         {
//             // Check if the hit object has a tag matching a plant
//             string plantTag = hit.collider.tag;
//             Plant plant = PlantLoader.GetPlantByTag(plantTag);

//             if (plant != null)
//             {
//                 // Update the UI with plant details
//                 uiManager.UpdatePlantDetails(plant);
//             }
//         }
//     }
// }

using UnityEngine;

public class PlantRaycaster : MonoBehaviour
{
    public float rayDistance = 10f; // Adjust the distance based on your needs
    public UI_Manager uiManager;

    void Update()
    {
        // Shoot a raycast from the Pokedex camera
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            // Check if the hit object has a tag matching a plant
            string plantTag = hit.collider.tag;
            Plant plant = PlantLoader.GetPlantByTag(plantTag);

            if (plant != null)
            {
                // Update the UI with plant details
                uiManager.UpdatePlantDetails(plant);
                Debug.Log($"Detected Plant: {plant.name}");
            }
        }
        else
        {
            // Clear the UI when no plant is detected
            uiManager.ClearDetails();
        }
    }
}
