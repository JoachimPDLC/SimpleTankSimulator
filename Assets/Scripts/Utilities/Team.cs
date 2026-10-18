using UnityEngine;

public enum Teams : int
{
    kPlayers,
    kEnemies,
    kCount
};

public class Team : MonoBehaviour
{
    [SerializeField] private Teams m_team;

    public Teams GetTeam() { return m_team; }
}
