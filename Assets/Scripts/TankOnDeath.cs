using UnityEngine;

public class TankOnDeath : OnDeathBase
{
    public override void OnDeath()
    {
        if (m_owner.TryGetComponent(out TankController controller))
        {
            Level.GetEntitiesManager().RemoveEntity(
                controller.GetTeam(), controller.GetTank());
        }
        Destroy(m_owner);
    }
}
