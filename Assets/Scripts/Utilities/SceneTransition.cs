using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Animator m_transition;
    [SerializeField] private float m_transitionTime = 1f;
    [SerializeField] private string kTransitionAnimationTrigger = "Start";

    public void Transition(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Tried transitioning to" +
                " an empty or null sceneName!");
            return;
        }
        
        StartCoroutine(InnerTransition(sceneName));
    }

    private IEnumerator InnerTransition(string sceneName)
    {
        m_transition.SetTrigger(kTransitionAnimationTrigger);
        yield return new WaitForSeconds(m_transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
