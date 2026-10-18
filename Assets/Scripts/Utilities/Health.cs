using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private uint kStartingHealth = 1;
    [SerializeField] private bool m_Invincible = false;
    [SerializeField] private OnDeathBase[] m_onDeathEffects;
    private int m_health;

    private void Start()
    {
        m_health = (int)kStartingHealth;
    }

    public void ChangeHealth(int amount)
    {
        if (m_Invincible)
            return;

        m_health += amount;
        
        if (m_health <= 0)
        {
            foreach(OnDeathBase onDeath in m_onDeathEffects)
                onDeath.OnDeath();
        }
    }
}
