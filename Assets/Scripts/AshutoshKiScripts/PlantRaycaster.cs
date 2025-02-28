using UnityEngine;

public class PlantRaycaster : MonoBehaviour
{
    public float rayDistance = 10f; // Adjust as needed
    private UI_Manager uiManager;

    void Start()
    {
        GameObject uiManagerObj = GameObject.FindGameObjectWithTag("UIManager");
        if (uiManagerObj != null)
        {
            uiManager = uiManagerObj.GetComponent<UI_Manager>();
        }
        else
        {
            Debug.LogError("UI Manager not found in scene! Make sure it's tagged correctly.");
        }
    }

    void Update()
    {
        // Shoot a raycast from the Pokedex camera
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red); // 🔴 Visual Debug

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Debug.Log($"\U0001F33F Raycast hit: {hit.collider.name} (Tag: {hit.collider.tag})");

            // Ensure it's a plant
            if (!string.IsNullOrEmpty(hit.collider.tag) && hit.collider.tag != "Untagged")
            {
                Plant plant = PlantLoader.GetPlantByTag(hit.collider.tag);
                if (plant != null)
                {
                    Debug.Log($"✅ Found Plant Data: {plant.name}");
                    uiManager?.UpdatePlantDetails(plant);
                }
                else
                {
                    Debug.LogWarning($"⚠️ No plant found with tag: {hit.collider.tag}");
                }
            }
            else
            {
                Debug.Log("⚠️ Object hit is Untagged, skipping.");
            }
        }
        else
        {
            Debug.Log("❌ Raycast didn't hit anything.");
        }
    }
}
