using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private Tank[] m_players;
    [SerializeField] private AITankController[] m_enemyControllers;
    [SerializeField] private LevelUI m_UI;
    [SerializeField] private string kLeftInput = "KeyboardLeft";
    [SerializeField] private string kRightInput = "KeyboardRight";
    private EntitiesManager m_entitiesManager = new();
    static private Level m_instance;
    static public bool m_gameEnded { get; private set; } = false;
    static public bool m_gameStarted { get; private set; } = false;

    public void StartGame()
    {
        m_gameStarted = true;
    }

    private void Awake()
    {
        m_entitiesManager.ResetManager();
        m_gameEnded = false;
        m_instance = this;

        // Diable excess players.
        int numPlayers = PlayersManager.GetNumPlayers();
        for (int i = numPlayers; i < m_players.Length; i++)        
            m_players[i].TankSetActive(false);
    }

    public static EntitiesManager GetEntitiesManager()
    {
        return m_instance.m_entitiesManager;
    }

    private void Start()
    {
        m_gameStarted = false;
        m_gameEnded = false;        
        m_entitiesManager.m_onAllEnemiesKilled += OnPlayerWin;
        m_entitiesManager.m_onAllPlayersKilled += OnPlayerLose;
    }

    private void OnEnable()
    {
        PairPlayerDevices();
    }

    private void PairPlayerDevices()
    {
        if (m_players[0].TryGetComponent(
            out PlayerInput p1Input))
        {
            Debug.Log(p1Input.user);
            p1Input.user.UnpairDevices();
            InputUser.PerformPairingWithDevice(Keyboard.current, p1Input.user);
            p1Input.user.ActivateControlScheme(kLeftInput);
        }
        else
        {
            Debug.LogError("No PlayerInput component on player 1!");
            return;
        }
        

        if (PlayersManager.GetNumPlayers() == 2)
        {
            if (m_players[1].TryGetComponent(
            out PlayerInput p2Input))
            {
                p2Input.user.UnpairDevices();
                InputUser.PerformPairingWithDevice(Keyboard.current, p2Input.user);
                p2Input.user.ActivateControlScheme(kRightInput);
            }
            else
            {
                Debug.LogError("No PlayerInput component on player 2!");
                return;
            }
        }
    }

    private void OnPlayerWin()
    {
        if (m_gameEnded)
            return;

        Time.timeScale = 0.3f;
        m_gameEnded = true;
        string sceneName = SceneManager.GetActiveScene().name;
        LevelManager.AddLevelCompletion(sceneName);
        LevelManager.Save();
        m_UI.OnGameEnd(didPlayerWin: true);
    }

    private void OnPlayerLose()
    {
        if (m_gameEnded)
            return;

        Time.timeScale = 0.3f;
        m_gameEnded = true;
        m_UI.OnGameEnd(didPlayerWin: false);
    }
}
