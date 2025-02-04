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
    public InputActionProperty moveCloserAction;  // A button (move object closer)
    public InputActionProperty moveFartherAction; // B button (move object farther)
    public float grabOffset = 5f;
    public float cloneScale = 0.5f; // Scale of cloned object (modifiable in editor)
    public float moveStep = 0.2f; // How much to move per button press
    public float minGrabOffset = 1f; // Min distance from controller
    public float maxGrabOffset = 10f; // Max distance from controller

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
        HandleObjectDistance(); // New function to move object closer/farther
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

    void HandleObjectDistance()
    {
        if (isHolding && grabbedObject != null)
        {
            // Move object closer when A button is pressed
            if (moveCloserAction.action.WasPressedThisFrame())
            {
                grabOffset -= moveStep;
                grabOffset = Mathf.Clamp(grabOffset, minGrabOffset, maxGrabOffset); // Prevent extreme values
            }

            // Move object farther when B button is pressed
            if (moveFartherAction.action.WasPressedThisFrame())
            {
                grabOffset += moveStep;
                grabOffset = Mathf.Clamp(grabOffset, minGrabOffset, maxGrabOffset);
            }
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
                    // Pick up an already cloned object
                    grabbedObject = floatingObject.gameObject;
                }
                else
                {
                    // Create a new clone
                    grabbedObject = Instantiate(gameObject, rightHandRay.position + rightHandRay.forward * grabOffset, rightHandRay.rotation);
                    grabbedObject.transform.localScale = Vector3.one * cloneScale; // Set clone scale

                    FloatingObject cloneScript = grabbedObject.GetComponent<FloatingObject>();
                    cloneScript.isClone = true;
                }

                Rigidbody grabbedRb = grabbedObject.GetComponent<Rigidbody>();
                grabbedRb.isKinematic = true;
                grabbedRb.useGravity = false; // Turn off gravity when picked up

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
            grabbedRb.useGravity = true; // Turn on gravity when released
            grabbedObject = null;
        }
        isHolding = false;
    }
}
