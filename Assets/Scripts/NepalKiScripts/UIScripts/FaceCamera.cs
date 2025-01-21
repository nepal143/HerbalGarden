using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [Tooltip("Assign the camera this UI should face.")]
    public Camera targetCamera;

    void LateUpdate()
    {
        if (targetCamera == null)
        {
            Debug.LogWarning("No camera assigned to FaceCamera script. Please assign one in the Inspector.");
            return;
        }

        // Rotate the UI to face the camera
        transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward,
                         targetCamera.transform.rotation * Vector3.up);
    }
}
