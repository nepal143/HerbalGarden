using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public Transform player; // Assign XR Rig here
    public float height = 20f; // Height above the player
    public Vector3 offset = new Vector3(0, 20f, 0); // Offset from player position
    public bool rotateWithPlayer = false; // Toggle to rotate minimap with player

    void LateUpdate()
    {
        if (player == null) return;

        // Follow the player's position but maintain the height
        Vector3 newPosition = player.position + offset;
        transform.position = newPosition;

        // If rotateWithPlayer is enabled, match the player's Y rotation
        if (rotateWithPlayer)
        {
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Fixed top-down view
        }
    }
}
