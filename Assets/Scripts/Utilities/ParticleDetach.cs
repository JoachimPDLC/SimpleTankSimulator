using UnityEngine;

public class ParticleDetach : MonoBehaviour
{
    public ParticleSystem emitter;
    private Vector3 m_startingScale;

    private void Start()
    {
        m_startingScale = transform.localScale;
    }

    public void StopAndDetach()
    {        
        emitter.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Detach();
    }

    public void StartAndDetach()
    {
        emitter.Play(true);
        Detach();
    }

    public void Detach()
    {
        emitter.transform.parent = null;
        emitter.transform.localScale = m_startingScale;
        var main = emitter.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
    }
}
