using UnityEngine;

public class OffsetAfterInstantiation : MonoBehaviour
{
    public Vector3 positionOffset;  // Offset for position
    public Vector3 rotationOffset;  // Offset for rotation (Euler angles)

    void Start()
    {
        // Apply position offset
        transform.position += positionOffset;

        // Apply rotation offset
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationOffset);
    }
}
