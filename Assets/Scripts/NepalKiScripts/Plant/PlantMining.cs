using UnityEngine;

public class PlantMining : MonoBehaviour
{
    public int requiredHits = 3; // Number of hits required to break the plant
    private int currentHits = 0;
    private bool isBroken = false;
    public GameObject floatingPlantPrefab; // The plant item that will be spawned
    public float spawnOffsetY = 0.5f; // Offset to spawn above the ground
    public float randomForce = 2f; // Random force for floating effect

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.gameObject.name); // Debugging

        if (other.CompareTag("Shovel") && !isBroken)
        {
            currentHits++;
            Debug.Log("Shovel hit! Current hits: " + currentHits); // Debugging

            if (currentHits >= requiredHits)
            {
                BreakPlant();
            }
        }
    }

    void BreakPlant()
    {
        isBroken = true;

        // Spawn the floating plant
        GameObject floatingPlant = Instantiate(floatingPlantPrefab, transform.position + Vector3.up * spawnOffsetY, Quaternion.identity);

        // Apply physics to make it float a little
        Rigidbody rb = floatingPlant.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.AddForce(new Vector3(Random.Range(-randomForce, randomForce), randomForce, Random.Range(-randomForce, randomForce)), ForceMode.Impulse);
        }

        // Destroy the original plant
        Destroy(gameObject);
    }
}
