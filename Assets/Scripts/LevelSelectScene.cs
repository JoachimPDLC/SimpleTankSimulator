using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScene : MonoBehaviour
{
    [SerializeField] private SceneRef kMainMenuScene;
    [SerializeField] private Button m_deleteSaveButton;
    [SerializeField] private SceneTransition m_transition;
    [SerializeField] private LevelSelector[] m_levelSelectors;

    private void Start()
    {
        LevelManager.Load();
        RefreshSelectors();
    }

    public void OnMainMenuButtonPressed()
    {
        if (kMainMenuScene != null)
            m_transition.Transition(kMainMenuScene.Name);
    }

    public void OnDeleteSavePressed()
    {
        LevelManager.ResetCompletion();
        RefreshSelectors();
    }

    private void RefreshSelectors()
    {
        m_deleteSaveButton.interactable = LevelManager.DoesSaveExist();
        foreach (var levelSelector in m_levelSelectors)
            levelSelector.Refresh();
    }
}
