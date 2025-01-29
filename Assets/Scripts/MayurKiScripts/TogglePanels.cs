using UnityEngine;

public class TogglePanels : MonoBehaviour
{
    [Header("Panel References")]
    [Tooltip("The panel to deactivate when the button is pressed.")]
    public GameObject panelToDisable;

    [Tooltip("The panel to activate when the button is pressed.")]
    public GameObject panelToActivate;

    public void SwitchPanels()
    {
        if (panelToDisable != null)
        {
            panelToDisable.SetActive(false);
        }

        if (panelToActivate != null)
        {
            panelToActivate.SetActive(true);
        }
    }
}
