using UnityEngine;

public class PlayerReloadBar : MonoBehaviour
{
    [SerializeField] private TankTurret m_turret;
    [SerializeField] private SliderBar m_bar;

    void Update()
    {
        m_bar.SetValue(m_turret.GetNormalizedReloadTime());
    }
}
