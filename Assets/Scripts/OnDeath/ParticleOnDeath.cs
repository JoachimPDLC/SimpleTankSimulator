using UnityEngine;

public class ParticleOnDeath : OnDeathBase
{
    [SerializeField] private ParticleDetach m_particleDetach;
    public override void OnDeath()
    {
        m_particleDetach.StartAndDetach();
    }
}
