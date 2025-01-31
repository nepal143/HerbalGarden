using UnityEngine;
using UnityEngine.InputSystem;

public class FloatingObject : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatMagnitude = 0.5f;
    public float floatSpeed = 1.0f;

    [Header("VR Gaze Interaction")]
    public Transform vrController;
    public float gazeDistance = 10f;
    public float multiplyScale = 1.1f;

    [Header("VR Grab Settings")]
    public Transform rightHandRay;
    public InputActionProperty grabAction;
    public float grabOffset = 5f;

    private Vector3 startPosition;
    private Vector3 originalScale;
    private bool isScaled = false;
    private GameObject grabbedObject = null;
    private bool isHolding = false;
    private bool isClone = false;

    private Rigidbody rb;

    void Start()
    {
        startPosition = transform.position;
        originalScale = transform.localScale;

        rb = GetComponent<Rigidbody>();

        // Disable floating if it's a clone
        if (isClone)
        {
            enabled = false;
        }
    }

    void Update()
    {
        if (!isClone)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatMagnitude;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        }

        HandleVRGazeScaling();
        HandleVRGrab();
    }

    void HandleVRGazeScaling()
    {
        bool isLookingAtObject = false;

        if (vrController != null)
        {
            Ray ray = new Ray(vrController.position, vrController.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, gazeDistance))
            {
                if (hit.transform == transform)
                {
                    isLookingAtObject = true;
                }
            }
        }

        if (isLookingAtObject && !isScaled)
        {
            transform.localScale = originalScale * multiplyScale;
            isScaled = true;
        }
        else if (!isLookingAtObject && isScaled)
        {
            transform.localScale = originalScale;
            isScaled = false;
        }
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
        Ray ray = new Ray(vrController.position, vrController.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, gazeDistance))
        {
            FloatingObject floatingObject = hit.transform.GetComponent<FloatingObject>();

            if (floatingObject != null)
            {
                if (floatingObject.isClone)
                {
                    // Pick up the already cloned object
                    grabbedObject = floatingObject.gameObject;
                }
                else
                {
                    // Create a new clone
                    grabbedObject = Instantiate(gameObject, rightHandRay.position + rightHandRay.forward * grabOffset, rightHandRay.rotation);
                    grabbedObject.transform.localScale = Vector3.one;

                    FloatingObject cloneScript = grabbedObject.GetComponent<FloatingObject>();
                    cloneScript.isClone = true;
                }

                Rigidbody grabbedRb = grabbedObject.GetComponent<Rigidbody>();
                grabbedRb.isKinematic = true;

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
            grabbedObject = null;
        }
        isHolding = false;
    }
}
