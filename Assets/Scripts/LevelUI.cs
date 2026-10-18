using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private GameObject m_endGameUI;
    [SerializeField] private TextMeshProUGUI m_endGameTMP;
    [SerializeField] private string m_winText;
    [SerializeField] private string m_loseText;
    [SerializeField] private float kStartDelay = 4f;
    [SerializeField] private GameObject m_startGameUI;
    [SerializeField] private TextMeshProUGUI m_startGameTMP;
    [SerializeField] private string m_startGameHoldText;
    [SerializeField] private string m_startGameGoText;
    [SerializeField] private float kStartUITime = 1f;
    [SerializeField] private PlayerReloadBar m_player2ReloadBar;
    [SerializeField] private SceneRef kMainMenuScene;
    [SerializeField] private SceneRef kLevelSelectScene;
    [SerializeField] private Level m_level;
    [SerializeField] private SceneTransition m_transition;

    private void Awake()
    {
        m_endGameUI.SetActive(false);
        if (PlayersManager.GetNumPlayers() == 1)        
            m_player2ReloadBar.gameObject.SetActive(false);        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0.0f;
        m_startGameUI.SetActive(true);
        m_startGameTMP.text = m_startGameHoldText;
        StartCoroutine(StartStartDelay());
    }

    private IEnumerator StartStartDelay()
    {
        yield return new WaitForSecondsRealtime(kStartDelay);
        Time.timeScale = 1.0f;
        m_startGameTMP.text = m_startGameGoText;
        StartCoroutine(HideStartUI());
    }

    private IEnumerator HideStartUI()
    {
        yield return new WaitForSeconds(kStartUITime);
        m_startGameUI.SetActive(false);
        m_level.StartGame();
    }

    public void OnGameEnd(bool didPlayerWin)
    {
        if (didPlayerWin)        
            m_endGameTMP.text = m_winText;
        else
            m_endGameTMP.text = m_loseText;
        m_endGameUI.SetActive(true);
    }

    public void OnMainMenuButtonPressed()
    {
        if (kMainMenuScene != null)
            m_transition.Transition(kMainMenuScene.Name);
    }

    public void OnLevelSelectButtonPressed()
    {
        if (kLevelSelectScene != null)
            m_transition.Transition(kLevelSelectScene.Name);
    }

    public void OnRestartButtonPressed()
    {
        m_transition.Transition(SceneManager.GetActiveScene().name);
    }
}
