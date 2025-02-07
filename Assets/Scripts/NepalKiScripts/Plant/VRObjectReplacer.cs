using UnityEngine;
using UnityEngine.InputSystem;

public class VRObjectReplacer : MonoBehaviour
{
    [Header("VR Grab Settings")]
    public Transform rightHandRay;
    public InputActionProperty grabAction;
    public float grabOffset = 5f;
    public float moveStep = 0.2f;
    public float minGrabOffset = 1f;
    public float maxGrabOffset = 10f;

    private GameObject grabbedObject = null;
    private bool isHolding = false;

    void Start()
    {
        // Automatically find the right hand ray using the "RightHandRay" tag
        GameObject rightHandRayObject = GameObject.FindGameObjectWithTag("RightHandRay");
        if (rightHandRayObject != null)
        {
            rightHandRay = rightHandRayObject.transform;
        }
        else
        {
            Debug.LogError("RightHandRay object not found! Make sure it has the correct tag.");
        }
    }

    void Update()
    {
        HandleVRGrab();
    }

    void HandleVRGrab()
    {
        if (grabAction.action.WasPressedThisFrame())
        {
            TryGrabObject();
        }

        if (isHolding && grabbedObject != null)
        {
            grabbedObject.transform.position = rightHandRay.position + rightHandRay.forward * grabOffset;
            grabbedObject.transform.rotation = rightHandRay.rotation;
        }

        if (grabAction.action.WasReleasedThisFrame() && isHolding)
        {
            ReleaseObject();
        }
    }

    void TryGrabObject()
    {
        Ray ray = new Ray(rightHandRay.position, rightHandRay.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            GameObject targetObject = hit.transform.gameObject;

            if (targetObject.CompareTag("Grabbable")) // Ensure the object is tagged as grabbable
            {
                grabbedObject = targetObject;
                Rigidbody grabbedRb = grabbedObject.GetComponent<Rigidbody>();
                grabbedRb.isKinematic = true;
                grabbedRb.useGravity = false;
                isHolding = true;
            }
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            Rigidbody grabbedRb = grabbedObject.GetComponent<Rigidbody>();
            grabbedRb.isKinematic = false;
            grabbedRb.useGravity = true;
            grabbedObject = null;
        }
        isHolding = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (grabbedObject != null && other.CompareTag("InventoryItem"))
        {
            Debug.Log("Touched an Inventory Item: " + other.name);
            // Later, you can add logic to replace or interact with inventory
        }
    }
}