using UnityEngine;

public abstract class OnDeathBase : MonoBehaviour
{
    protected GameObject m_owner;
    public abstract void OnDeath();

    public void Start()
    {
        m_owner = gameObject;
    }
}
