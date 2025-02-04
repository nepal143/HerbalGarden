using UnityEngine;

public class HoveringObject : MonoBehaviour
{
    public float hoverHeight = 1.5f; // Desired height above the terrain
    public float hoverForce = 5f; // Upward force to keep object floating
    public float bobbingSpeed = 2f; // Speed of bobbing motion
    public float bobbingAmount = 0.2f; // How much the object bobs
    public float rotationSpeed = 30f; // Rotating effect

    private Rigidbody rb;
    private float initialY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody found! Adding one.");
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false; // Disable gravity to prevent falling
        initialY = transform.position.y;
    }

    void FixedUpdate()
    {
        // Raycast to detect terrain height below the object
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
        {
            float terrainHeight = hit.point.y;
            float targetY = terrainHeight + hoverHeight;

            // Apply force to maintain hovering
            float force = (targetY - transform.position.y) * hoverForce;
            rb.velocity = new Vector3(rb.velocity.x, force, rb.velocity.z);
        }

        // Apply bobbing motion (optional)
        float bobbingOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        rb.MovePosition(new Vector3(transform.position.x, transform.position.y + bobbingOffset, transform.position.z));

        // Rotate the object slowly
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
