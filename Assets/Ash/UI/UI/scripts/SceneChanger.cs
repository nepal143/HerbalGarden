using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneChanger : MonoBehaviour
{
    public string sceneName; // Name of the scene to load

    public void ChangeScene()
    {
        // Get the current active scene name
        string currentScene = SceneManager.GetActiveScene().name;

        // Check if the sceneName is set and is different from the current scene
        if (!string.IsNullOrEmpty(sceneName) && sceneName != currentScene)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is not set or is the same as the current scene!");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
