using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] protected Tank m_tank;
    [SerializeField] protected TankTurret m_turret;
    [SerializeField] protected Teams m_team;
    [SerializeField] protected Teams m_targetTeam;

    public Tank GetTank() { return m_tank; }
    public Teams GetTeam() { return m_team; }
    public Teams GetTargetTeam() { return m_targetTeam; }
}
