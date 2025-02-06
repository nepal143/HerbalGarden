// using UnityEngine;

// public class SnapTracker : MonoBehaviour
// {
//     public GameObject snapToPoint; // Reference to the SnapToPoint GameObject
//     public GameObject snappedPlant; // This will store the snapped plant

//     void Update()
//     {
//         // Check if the snap point GameObject is disabled and if the snapped object is set
//         if (snapToPoint != null && !snapToPoint.activeInHierarchy)
//         {
//             // Get the snapped plant if the snap point is disabled
//             SnapToPoint snapToPointScript = snapToPoint.GetComponent<SnapToPoint>();
//             if (snapToPointScript != null && snapToPointScript.snappedObject != null)
//             {
//                 snappedPlant = snapToPointScript.snappedObject; // Get the snapped plant
//                 Debug.Log("Snapped plant tracked: " + snappedPlant.name);

//                 // Optionally reset the snappedObject if you want it to track a new snap
//                 snapToPointScript.snappedObject = null;
//             }
//         }
//     }
// }
