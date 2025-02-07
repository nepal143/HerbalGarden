using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapToHandOnGrab : XRGrabInteractable
{
    public float snapDistance = 0.5f; // Distance threshold for snapping

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Get the transform of the interactor (hand/controller)
        Transform interactorTransform = args.interactorObject.transform;

        // Calculate distance between object and hand
        float distance = Vector3.Distance(transform.position, interactorTransform.position);

        // If object is further than snapDistance, move it close
        if (distance > snapDistance)
        {
            transform.position = interactorTransform.position; // Snap to hand
        }

        // Always match rotation
        transform.rotation = interactorTransform.rotation;
    }
}
