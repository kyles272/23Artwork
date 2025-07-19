using UnityEngine;

public class Pause : MonoBehaviour
{

    public PauseAction action;
    
    private bool paused = false;

    public GameObject pauseMenu;

    

    private void Awake()
    {
        action = new PauseAction();
    }

    private void Start()
    {
        action.Pause.PauseGame.performed += _ => DeteminePause();
        pauseMenu.SetActive(false); // Ensure the pause menu is hidden at start
    }

    private void DeteminePause()
    {
        if (paused)
            ResumeGame();
        else
            PauseGame();
    }
    private void OnEnable()
    {
        action.Enable();
    }
    private void OnDisable()
    {
        action.Disable();
    }
    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        paused = true;
        pauseMenu.SetActive(true); // Show the pause menu
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        paused = false;
        pauseMenu.SetActive(false); // Hide the pause menu
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
    }
}
