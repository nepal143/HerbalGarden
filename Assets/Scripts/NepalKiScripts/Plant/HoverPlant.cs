using UnityEngine;

public class HoverPlant : MonoBehaviour
{
    public float floatHeight = 0.5f; // The height the object floats up and down
    public float floatSpeed = 1f; // The speed at which the object floats
    public float floatRotationSpeed = 30f; // Speed of the object’s rotation

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Sinusoidal movement for floating effect
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Optional: Add rotation for added realism
        transform.Rotate(Vector3.up * floatRotationSpeed * Time.deltaTime);
    }
}
