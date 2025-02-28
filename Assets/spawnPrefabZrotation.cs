using UnityEngine;

public class SpawnPrefabOnZRotation : MonoBehaviour
{
    [Tooltip("The hand object whose Z-axis rotation will be monitored.")]
    public Transform handObject;

    [Tooltip("The player reference to determine the spawn direction.")]
    public Transform player;

    [Tooltip("The prefab to spawn.")]
    public GameObject prefabToSpawn;

    [Tooltip("Check to enable Clockwise detection, uncheck for Anti-Clockwise.")]
    public bool isClockwise = false; // Default: Anti-Clockwise

    [Tooltip("Offset applied to the spawn position (X, Y, Z).")]
    public Vector3 spawnOffset;

    private bool hasSpawned = false; // Prevents multiple spawns per activation

    private void Update()
    {
        if (handObject == null || player == null || prefabToSpawn == null)
        {
            Debug.LogWarning("Hand object, player, or prefab is not assigned.");
            return;
        }

        // Get the hand object's local z-axis rotation
        float handRotationZ = handObject.localEulerAngles.z;

        // Adjust for Unity's 360-degree rotation wrapping
        if (handRotationZ > 180)
            handRotationZ -= 360;

        bool shouldSpawn = false;

        // Check rotation conditions
        if (isClockwise)
        {
            shouldSpawn = (handRotationZ >= 60 && handRotationZ <= 180);
        }
        else
        {
            shouldSpawn = (handRotationZ <= -60 && handRotationZ >= -180);
        }

        // Spawn prefab only when conditions are met and prevent multiple spawns
        if (shouldSpawn && !hasSpawned)
        {
            SpawnPrefab();
            hasSpawned = true; // Ensure it only spawns once per trigger
        }
        else if (!shouldSpawn)
        {
            hasSpawned = false; // Reset so it can trigger again
        }
    }

    private void SpawnPrefab()
    {
        // Calculate spawn position 5 meters in front of the player
        Vector3 spawnPosition = player.position + (player.forward * 5f) + spawnOffset;

        // Instantiate the prefab
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
}
  