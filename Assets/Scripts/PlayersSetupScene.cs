using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayersSetupScene : MonoBehaviour
{
    [SerializeField] private Image[] m_secondPlayerControlsLights;
    [SerializeField, Range(0f, 1f)]
    private float m_unlitLightAlpha;
    [SerializeField, Range(0f, 1f)]
    private float m_litLightAlpha = 1;
    private float m_lightFadeTime = 0.05f;
    [SerializeField] private Toggle m_secondPlayerToggle;
    [SerializeField] private SceneRef kMainMenuScene;
    [SerializeField] private SceneTransition m_transition;

    void Start()
    {
        float temp = m_lightFadeTime;
        m_lightFadeTime = 0;
        if (PlayersManager.GetNumPlayers() == 2)        
            m_secondPlayerToggle.isOn = true;        
        else        
            m_secondPlayerToggle.isOn = false;
        m_lightFadeTime = temp;
    }

    public void OnToggled()
    {
        if (m_secondPlayerToggle.isOn)
        {
            PlayersManager.AddPlayer();
            SetEachLight(alphaValue: m_litLightAlpha);
        }
        else
        {
            PlayersManager.RemovePlayer();
            SetEachLight(alphaValue: m_unlitLightAlpha);
        }
    }

    private void SetEachLight(float alphaValue)
    {
        foreach (Image light in m_secondPlayerControlsLights)        
            light.CrossFadeAlpha(alphaValue, m_lightFadeTime, false);
    }

    public void OnMainMenuButtonPressed()
    {
        if (m_secondPlayerToggle.isOn &&
            PlayersManager.GetNumPlayers() == 1)
        {
            PlayersManager.AddPlayer();
        }
        else if (!m_secondPlayerToggle.isOn &&
            PlayersManager.GetNumPlayers() == 2)
        {
            PlayersManager.RemovePlayer();
        }
        if (kMainMenuScene != null)
            m_transition.Transition(kMainMenuScene.Name);
    }
}
