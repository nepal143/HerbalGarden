using System.Collections.Generic;
using UnityEngine;

public class PlantSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] plantPrefabs; // Assign 10 herb prefabs
    [SerializeField] private Transform[] spawnPoints;   // Assign spawn points in the scene

    private List<GameObject> spawnedPlants = new List<GameObject>();

    void Start()
    {
        SpawnPlants();
    }

    public void SpawnPlants()
    {
        // Clear previously spawned plants
        foreach (var plant in spawnedPlants)
        {
            Destroy(plant);
        }
        spawnedPlants.Clear();

        // Spawn new plants with fixed rotation
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject selectedPlant = plantPrefabs[Random.Range(0, plantPrefabs.Length)];
            GameObject newPlant = Instantiate(selectedPlant, spawnPoint.position, Quaternion.Euler(-90, 0, 0));
            spawnedPlants.Add(newPlant);
        }
    }
}
