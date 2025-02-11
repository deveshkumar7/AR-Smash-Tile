using UnityEngine;
using UnityEngine.SceneManagement;  // For scene management

public class GameControl : MonoBehaviour
{
    public GameObject pauseMenu;  // Reference to the pause menu UI
    private bool isPaused = false;

    void Start()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);  // Ensure pause menu is hidden at the start
        }
    }

    void Update()
    {
        // Toggle pause when the "Escape" key is pressed (or any other key you prefer)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Pause the game
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Stop the game time
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);  // Show the pause menu
        }
    }

    // Resume the game
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;  // Resume the game time
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);  // Hide the pause menu
        }
    }

    // Reload the current scene
    public void ReloadScene()
    {
        Time.timeScale = 1f;  // Ensure the game is not paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }

    // Exit the game
    public void ExitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // Stop playmode in the Unity Editor
#else
        Application.Quit();  // Quit the game when built
#endif
    }
}
