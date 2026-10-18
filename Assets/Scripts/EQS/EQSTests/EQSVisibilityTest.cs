using EQS;
using UnityEngine;

[CreateAssetMenu(fileName = "EQSVisibilityTest", menuName = "Scriptable Objects/EQSVisibilityTest")]

public class EQSVisibilityTest : EQSTest
{
    [SerializeField] private bool m_validIfVisible = false;
    [SerializeField] private Vector3[] m_visibilityTestPoints;
    [SerializeField] private LayerMask m_raycastMask;
    private RaycastHit[] m_hits = new RaycastHit[10];

    public override float Score(ref EQSQueryPoint point, in EQSContext context)
    {
        Vector3 targetPos = context.Target.position;

        Vector3 directionToTarget = targetPos - point.Position;

        foreach (Vector3 testPoint in m_visibilityTestPoints)
        {
            int numHits = Physics.RaycastNonAlloc(point.Position + testPoint,
                directionToTarget, m_hits, directionToTarget.magnitude, m_raycastMask);

            if (numHits <= 0)
                return 1;

            int hit = GetClosestHit(point.Position, numHits);
            
            // Quit if we hit the target. This
            // means the target can see this point,
            // meaning we are visible.
            if (m_hits[hit].collider.gameObject ==
                context.Target.gameObject)
            {
                if (!m_validIfVisible)
                {
                    // In this case, we want points
                    // the target can't see.
                    point.Valid = false;
                    return float.MinValue;
                }
            }
            else
            {
                // If we got to this part, we want points that
                // the target can see, but this point isn't
                // visible by the target, so set it as invalid.
                if (m_validIfVisible)
                {
                    point.Valid = false;
                    return float.MinValue;
                }
            }
        }
        
        return 1;
    }

    private int GetClosestHit(Vector3 position, int numHits)
    {
        if (numHits == 0)
        {
            Debug.LogWarning("GetClosestHit called when numHits" +
                " == 0! Returning 0.");
            return 0;
        }

        int closestHit = int.MaxValue;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < numHits; i++)
        {
            if (m_hits[i].point == Vector3.zero)
                continue;

            float distance =
                Vector3.Distance(position, m_hits[i].point);
            if (distance < closestDistance)
            {
                closestHit = i;
                closestDistance = distance;
            }
        }

        return closestHit;
    }
}
