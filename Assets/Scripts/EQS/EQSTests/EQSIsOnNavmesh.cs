using EQS;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "EQSIsOnNavmesh", menuName = "Scriptable Objects/EQSIsOnNavmesh")]

public class EQSIsOnNavmesh : EQSTest
{
    [SerializeField] private float kCheckDistance = 0.1f;
    private NavMeshHit m_hit;

    public override float Score(ref EQSQueryPoint point, in EQSContext context)
    {
        if (NavMesh.SamplePosition(point.Position,
            out m_hit, kCheckDistance, NavMesh.AllAreas))
        {
            return 1;
        }
        else
        {
            point.Valid = false;
            return float.MinValue;
        }        
    }
}
