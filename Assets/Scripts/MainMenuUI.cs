using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private SceneRef kLevelSelectScene;
    [SerializeField] private SceneRef kPlayersSetupScene;
    [SerializeField] private SceneTransition m_transition;

    public void LevelSelectPressed()
    {
        if (kLevelSelectScene != null)
            m_transition.Transition(kLevelSelectScene.Name);
    }

    public void PlayersSetupPressed()
    {
        if (kPlayersSetupScene != null)
            m_transition.Transition(kPlayersSetupScene.Name);
    }

    public void OnQuitButtonPressed()
    {
        Utilities.Quit();
    }
}
