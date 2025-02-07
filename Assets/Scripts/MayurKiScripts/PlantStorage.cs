using UnityEngine;
using System.Collections.Generic;

public class PlantStorage : MonoBehaviour
{
    public List<GameObject> storedPlants = new List<GameObject>(); // List to store plants

    // Function to add a plant to the storage
    public void AddPlant(GameObject plant)
    {
        if (plant != null && !storedPlants.Contains(plant)) 
        {
            storedPlants.Add(plant);
            Debug.Log("Added plant to storage: " + plant.name);
        }
    }
}
