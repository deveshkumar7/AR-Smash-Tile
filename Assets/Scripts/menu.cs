using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Function to load a new scene by name
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is invalid or empty.");
        }
    }

    // Function to exit the application
    public void ExitApplication()
    {
        Debug.Log("Exiting application.");
        Application.Quit();
    }
}
