using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Canvas m_selector;
    [SerializeField] private Button m_button;
    [SerializeField] private Material m_completedMaterial;
    [SerializeField] private SceneRef m_scene;
    [SerializeField] private SceneRef[] m_requiredLevels;
    [SerializeField] private SceneTransition m_transition;
    private Image m_buttonImage;
    private Material m_startingImageMat;

    private void Start()
    {
        if (m_button.TryGetComponent(out m_buttonImage))
            m_startingImageMat = m_buttonImage.material;        
        else
            Debug.LogWarning("No image for the selector: " + gameObject.name);        
        Refresh();
    }

    public void Refresh()
    {
        if (m_buttonImage == null)
            return;

        if (LevelManager.IsLevelCompleted(m_scene.Name))
        {
            m_button.interactable = true;
            m_buttonImage.material = m_completedMaterial;
            return;
        }

        // The level is not completed, so reset the 
        // color.
        m_buttonImage.material = m_startingImageMat;

        bool available = true;
        foreach (SceneRef level in m_requiredLevels)
        {
            if (!LevelManager.IsLevelCompleted(level.Name))
            {
                available = false;
                break;
            }
        }
        m_button.interactable = available;
    }

    public void OnLevelSelected()
    {
        if (m_scene != null)
            m_transition.Transition(m_scene.Name);
    }
}
