// using UnityEngine;

// public class PlantLoader : MonoBehaviour
// {
//     public static PlantList plantList;

//     void Awake()
//     {
//         // Load JSON data from Resources folder
//         TextAsset jsonData = Resources.Load<TextAsset>("PlantsData");
//         plantList = JsonUtility.FromJson<PlantList>(jsonData.text);
//     }

//     public static Plant GetPlantByTag(string tagName)
//     {
//         return plantList.plants.Find(plant => plant.name == tagName);
//     }
// }


using UnityEngine;

public class PlantLoader : MonoBehaviour
{
    public static PlantList plantList;

    void Awake()
    {
        // Load JSON data from Resources folder
        TextAsset jsonData = Resources.Load<TextAsset>("PlantsData");
        if (jsonData != null)
        {
            plantList = JsonUtility.FromJson<PlantList>(jsonData.text);
            Debug.Log($"Loaded {plantList.plants.Count} plants from JSON.");
        }
        else
        {
            Debug.LogError("Failed to load PlantsData.json from Resources.");
        }
    }

    public static Plant GetPlantByTag(string tagName)
    {
        if (plantList != null && plantList.plants != null)
        {
            return plantList.plants.Find(plant => plant.name == tagName);
        }
        return null;
    }
}
