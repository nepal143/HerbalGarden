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
    public GameObject infoPanel;

    [Header("VR Grab Settings")]
    public Transform rightHandRay;
    public InputActionProperty grabAction;
    public InputActionProperty moveCloserAction;
    public InputActionProperty moveFartherAction;
    public float grabOffset = 5f;
    public float cloneScale = 0.5f;
    public float moveStep = 0.2f;
    public float minGrabOffset = 1f;
    public float maxGrabOffset = 10f;

    [Header("Plant Model Settings")]
    public GameObject plantModel;
    public float modelScaleFactor = 1.0f;

    private Vector3 startPosition;
    private Vector3 originalScale;
    private bool isScaled = false;
    private GameObject spawnedModel;
    private GameObject grabbedObject = null;
    private bool isHolding = false;
    private bool isClone = false;

    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    void Start()
    {
        startPosition = transform.position;
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();

        // 🔹 Always keep rotation (0,0,0)
        transform.rotation = Quaternion.Euler(0, 0+90, 0);

        if (isClone)
            enabled = false;

        if (infoPanel != null)
            infoPanel.SetActive(false);

        // If plantModel is provided, disable the original mesh and spawn the model
        if (plantModel != null)
        {
            if (meshRenderer != null)
                meshRenderer.enabled = false;

            spawnedModel = Instantiate(plantModel, transform.position, Quaternion.Euler(0, 0+90, 0));
            spawnedModel.transform.localScale *= modelScaleFactor;
            spawnedModel.transform.parent = transform;

            // 🔹 Keep spawned model's rotation (0,0,0)
            spawnedModel.transform.rotation = Quaternion.Euler(0, 0+90, 0);
        }
    }

    void Update()
    {
        // 🔹 Force rotation to always stay (0,0,0)
        transform.rotation = Quaternion.Euler(0, 0+90, 0);

        if (!isClone)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatMagnitude;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        }

        HandleVRGazeScaling();
        HandleVRGrab();
        HandleObjectDistance();
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

        if (infoPanel != null)
            infoPanel.SetActive(isLookingAtObject && !isHolding);
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
            grabbedObject.transform.rotation = Quaternion.Euler(0, 0+90, 0); // 🔹 Always (0,0,0)
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
            if (moveCloserAction.action.WasPressedThisFrame())
            {
                grabOffset -= moveStep;
                grabOffset = Mathf.Clamp(grabOffset, minGrabOffset, maxGrabOffset);
            }

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
                    grabbedObject = floatingObject.gameObject;
                }
                else
                {
                    grabbedObject = Instantiate(hit.transform.gameObject, rightHandRay.position + rightHandRay.forward * grabOffset, Quaternion.Euler(0, 0+90, 0));
                    grabbedObject.transform.localScale = Vector3.one * cloneScale;

                    FloatingObject cloneScript = grabbedObject.GetComponent<FloatingObject>();
                    cloneScript.isClone = true;
                }

                Rigidbody grabbedRb = grabbedObject.GetComponent<Rigidbody>();
                grabbedRb.isKinematic = true;
                grabbedRb.useGravity = false;

                isHolding = true;

                if (infoPanel != null)
                    infoPanel.SetActive(false);
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
            grabbedObject.transform.rotation = Quaternion.Euler(0, 0+90, 0); // 🔹 Always (0,0,0)
            grabbedObject = null;
        }
        isHolding = false;
    }
}
