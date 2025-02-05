using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneChanger : MonoBehaviour
{
    // Name of the scene you want to load
    public string sceneName;

    // This function will be triggered when the button is pressed
    public void ChangeScene()
    {
        // Check if the scene name is not empty or null
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is not set in the inspector!");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
