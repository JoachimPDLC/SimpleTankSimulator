using UnityEngine;

public class SoundOnDeath : OnDeathBase
{
    [SerializeField] private AudioSource m_audioSource;

    public override void OnDeath()
    {
        SFXPlayer.Play(m_audioSource);
    }
}
