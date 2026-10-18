using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTankController : TankController
{
    private void Start()
    {
        Level.GetEntitiesManager().AddEntity(m_team, m_tank);
    }

    public void OnTankControl(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        m_tank.m_input.Forward = input.y;
        m_tank.m_input.Turning = input.x;
    }

    public void OnTurretTurn(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        m_turret.m_input.Turning = input;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)        
            m_turret.m_input.Shooting = true;
        else
            m_turret.m_input.Shooting = false;
    }
}
