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


using System.IO;
using UnityEngine;

public class PlantLoader : MonoBehaviour
{
    public static PlantList plantList;

    void Awake()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "PlantsData.json");

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            plantList = JsonUtility.FromJson<PlantList>(jsonData);

            Debug.Log("✅ JSON Loaded from StreamingAssets!");
            Debug.Log($"🌿 Total Plants Loaded: {plantList.plants.Count}");
        }
        else
        {
            Debug.LogError($"❌ JSON file not found at: {filePath}");
        }
    }

    public static Plant GetPlantByTag(string tagName)
    {
        return plantList?.plants?.Find(plant => plant.name == tagName);
    }
}
