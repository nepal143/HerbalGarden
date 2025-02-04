using UnityEngine;

public class AttachToShovel : MonoBehaviour
{
    private Transform shovelTransform;

    void Update()
    {
        if (shovelTransform == null) // Keep searching if shovel is not found
        {
            GameObject shovelObject = GameObject.FindGameObjectWithTag("Shovel");
            if (shovelObject != null)
            {
                shovelTransform = shovelObject.transform; // Assign shovel transform
                AttachObjectToShovel();
            }
        }
    }

    void AttachObjectToShovel() // ✅ Fixed: Renamed method
    {
        if (shovelTransform != null)
        {
            transform.SetParent(shovelTransform); // Parent this object to the shovel
            transform.localPosition = Vector3.zero; // Reset position relative to the shovel
            transform.localRotation = Quaternion.identity; // Reset rotation
        }
    }
}
