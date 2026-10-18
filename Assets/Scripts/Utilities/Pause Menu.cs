using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool s_isGamePaused { get; private set; } = false;
    [SerializeField] private GameObject m_pauseUI;
    [SerializeField] private SceneRef kLevelSelectScene;
    [SerializeField] private SceneTransition m_transition;

    private void Start()
    {
        m_pauseUI.SetActive(false);
    }

    public void TogglePause(InputAction.CallbackContext context)        
    {
        if (!Level.m_gameStarted || Level.m_gameEnded)
            return;

        if (context.performed)
        {
            if (s_isGamePaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        m_pauseUI.SetActive(false);
        Time.timeScale = 1.0f;
        s_isGamePaused = false;
    }

    public void Pause()
    {
        m_pauseUI.SetActive(true);
        Time.timeScale = 0.0f;
        s_isGamePaused = true;
    }

    public void OnLevelSelectButtonPressed()
    {
        Time.timeScale = 1.0f;
        if (kLevelSelectScene != null)
            m_transition.Transition(kLevelSelectScene.Name);
    }

    public void OnRestartButtonPressed()
    {
        Time.timeScale = 1.0f;
        m_transition.Transition(SceneManager.GetActiveScene().name);
    }

    public void OnQuitButtonPressed()
    {
        Utilities.Quit();
    }
}
