using UnityEngine;

public class MinimapActivator : MonoBehaviour
{
    public Transform rightHand; // Assign the player's right hand/controller
    public CanvasGroup minimapCanvasGroup; // Assign the CanvasGroup of the minimap
    public float activationAngle = 45f; // Angle at which the minimap appears
    public float fadeSpeed = 2f; // Speed of fade in/out

    private bool isMinimapVisible = false;
    private float targetAlpha = 0f;

    void Start()
    {
        if (minimapCanvasGroup != null)
        {
            minimapCanvasGroup.alpha = 0f; // Ensure minimap starts invisible
        }
    }

    void Update()
    {
        if (rightHand == null || minimapCanvasGroup == null) return;

        // Get the rotation of the right hand
        float handTilt = rightHand.eulerAngles.z;
        if (handTilt > 180f) handTilt -= 360f;

        // Check if hand is tilted enough to show/hide the minimap
        if (handTilt > activationAngle)
        {
            targetAlpha = 1f; // Fade in
        }
        else
        {
            targetAlpha = 0f; // Fade out
        }

        // Smoothly adjust alpha
        minimapCanvasGroup.alpha = Mathf.Lerp(minimapCanvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
    }
}
