using UnityEngine;
using UnityEngine.AI;

public class AITankController : TankController
{
    [SerializeField] private bool m_doesNotMove = false;
    [SerializeField] private EQS.EQS m_EQS;
    [SerializeField] private NavMeshAgent m_navAgent;
    [SerializeField] private float m_stopDistance = 1.0f;
    [SerializeField] private float m_slowDistance = 30.0f;
    [SerializeField] private float m_navMeshSampleDistance = 5f;
    private Transform m_tankTransform;
    private Transform m_turretTransform;
    private NavMeshPath m_navMeshPath;

    private Tank m_target;
    private Transform m_targetTransform;
    [SerializeField] private float m_scanSize = 45.0f;
    [SerializeField] private int m_numScans = 35;
    [SerializeField] private float m_scanLength = 2000f;
    [SerializeField] private float m_scanSphereRadius = 0.175f;
    [SerializeField] private ProjectileParams m_projectileParams;
    [SerializeField] private float m_attackAngleTolerance;
    [SerializeField] private float m_turretStopTurnAngle = 2.0f;
    [SerializeField] private float m_turretSlowTurnAngle = 30.0f;
    [SerializeField] private float m_targetPredictionTime;
    private Transform m_projectileSpawn;
    private EntitiesManager m_entitiesManager;
    private RaycastHit[] m_hits = new RaycastHit[10];

    private void Start()
    {
        m_projectileSpawn = m_turret.GetProjectileSpawn();
        m_navMeshPath = new();
        m_tankTransform = m_tank.transform;
        m_turretTransform = m_turret.transform;

        // Gets -1 or 1 randomly
        m_turret.m_input.Turning = Random.Range(0, 2) * 2 - 1;
        m_entitiesManager = Level.GetEntitiesManager();
        m_entitiesManager.AddEntity(m_team, m_tank);
    }

    private void Update()
    {
        GameObject targetingObject = UpdateTarget();
        if (targetingObject == null)
        {
            m_tank.m_input.Turning = 0;
            m_tank.m_input.Forward = 0;
            m_turret.m_input.Turning = 0;
            m_turret.m_input.Shooting = false;
            return;
        }

        if (!m_doesNotMove)
        {
            EQS.EQSResult result = m_EQS.Query();
            if (m_navMeshPath != null)
            {
                NavMesh.SamplePosition(result.Position, out NavMeshHit hit,
                    m_navMeshSampleDistance, NavMesh.AllAreas);
                NavMesh.CalculatePath(m_tankTransform.position,
                    hit.position, NavMesh.AllAreas, m_navMeshPath);
            }
            FollowPath();
        }
        UpdateTurret(targetingObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (m_navMeshPath != null && m_navMeshPath.corners != null)
        {
            for (int i = 1; i < m_navMeshPath.corners.Length; i++)
            {
                Debug.DrawLine(m_navMeshPath.corners[i - 1], m_navMeshPath.corners[i], Color.red);
            }
        }
    }

    private GameObject UpdateTarget()
    {
        if (m_target == null)
        {
            m_target = m_entitiesManager.GetTarget(m_tankTransform.position);
            if (m_target == null)
                return null;
            m_targetTransform = m_target.transform;
            m_EQS?.SetTarget(m_targetTransform);
        }
        GameObject targetingObject = m_target.GetTargetingObject();
        if (m_target == null || targetingObject == null)
            return null;
        m_target.SetTargetingObjectPos(m_targetPredictionTime);
        return targetingObject;
    }

    private void FollowPath()
    {
        Vector3 targetPos = m_navMeshPath.corners[1];
        MoveTowardsPoint(targetPos);
    }

    private void TurnTowardsPoint(Vector3 point)
    {
        Vector3 adjustedPoint = point;
        Vector3 tankPos = m_tankTransform.position;
        adjustedPoint.y = tankPos.y;
        Vector3 directionDifference = adjustedPoint - tankPos;

        if (directionDifference == Vector3.zero)
            return;

        float angle = Vector3.SignedAngle(
            m_tankTransform.forward, directionDifference, Vector3.up);

        if (angle > 0) // turn right
            m_tank.m_input.Turning = 1;
        else if (angle < 0) // turn left
            m_tank.m_input.Turning = -1;
        else
            m_tank.m_input.Turning = 0;
    }

    private void MoveTowardsPoint(Vector3 point)
    {
        float distance = Vector3.Distance(m_tankTransform.position, point);
        TurnTowardsPoint(point);

        float input = 1;

        if (distance < m_stopDistance)        
            input = 0;        
        
        if (distance < m_slowDistance)
            input = distance / m_slowDistance;

        m_tank.m_input.Forward = input;
    }

    private void UpdateTurret(GameObject targetingObject)
    {
        float turretAngle = m_turretTransform.localEulerAngles.y;
        if (turretAngle > 180)
            turretAngle -= 360;

        m_turret.m_input.Shooting = false;
        if (SphereScan(targetingObject,
            m_projectileSpawn.position,
            m_turretTransform.forward,
            m_projectileParams.NumBounces))
        {
            m_turret.m_input.Shooting = true;
            m_turret.m_input.Turning = 0;
            return;
        }

        if (FindAttackDirection(out Vector3 direction, targetingObject))
        {
            TurnTurretTowardsDirection(direction, turretAngle);
        }
        else if (m_turret.m_input.Turning == 0)
        {
            m_turret.m_input.Turning = Random.Range(0, 2) * 2 - 1;
        }
    }
    private bool FindAttackDirection(out Vector3 direction, GameObject targetingObject)
    {
        // Check if we have line of sight
        Vector3 directionToTarget = m_targetTransform.position - m_tankTransform.position;
        if (SphereScan(targetingObject, m_projectileSpawn.position,
                directionToTarget, 0))
        {
            direction = directionToTarget;
            return true;
        }

        float step = (m_scanSize * 2) / m_numScans;
        Vector3 scanDirection = m_turretTransform.forward;
        scanDirection = Quaternion.AngleAxis((-m_scanSize / 2),
            Vector3.up) * scanDirection;

        for (int i = 0; i < m_numScans; i++)
        {
            if (SphereScan(targetingObject, m_projectileSpawn.position,
                scanDirection, m_projectileParams.NumBounces))
            {
                direction = scanDirection;
                return true;
            }

            scanDirection = Quaternion.AngleAxis(step,
            Vector3.up) * scanDirection;
        }

        direction = default;
        return false;
    }

    private void TurnTurretTowardsDirection(Vector3 direction, float turretAngle)
    {
        Vector3 forward = m_turretTransform.forward;
        Vector3 delta = direction.normalized;
        Vector3 cross = Vector3.Cross(delta, forward);

        float absDiff = Vector3.Angle(forward, delta);

        if (absDiff <= m_turretStopTurnAngle ||
            cross == Vector3.zero)
        {
            m_turret.m_input.Turning = 0;
            return;
        }

        float input = 1;

        if (absDiff <= m_turretSlowTurnAngle)
        {
            input = absDiff / m_turretSlowTurnAngle;
        }

        if (cross.y > 0)
            m_turret.m_input.Turning = -input;
        else
            m_turret.m_input.Turning = input;
    }

    /// <summary>
    /// Returns true if the targeting object or the target
    /// is hit. Returns false if not.
    /// Handles bouncing through recursion.
    /// </summary>
    /// <param name="targetingObject"></param>
    /// <param name="start"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool SphereScan(GameObject targetingObject,
        Vector3 start, Vector3 direction, uint numBounces)
    {
        int numHits = Physics.SphereCastNonAlloc(start,
            m_scanSphereRadius, direction.normalized,
            m_hits, m_scanLength);

        if (numHits > 0)
        {
            int hitIndex = GetClosestHit(start, numHits);

            if (hitIndex == int.MaxValue)
                return false;

            GameObject collider = m_hits[hitIndex].collider.gameObject;

            Debug.Assert(m_hits[hitIndex].point != Vector3.zero);

            if (collider == targetingObject || collider == m_target)
            {
                Debug.DrawLine(start,
                    m_hits[hitIndex].point, Color.green);
                return true;
            }

            Debug.DrawLine(start,
                    m_hits[hitIndex].point, Color.purple);

            // The hit is not the target, so we can bounce
            // off of it.
            if (numBounces > 0)
            {
                Vector3 newDirection = Vector3.Reflect(direction,
                    m_hits[hitIndex].normal);
                --numBounces;
                return SphereScan(
                    targetingObject,
                    start: m_hits[hitIndex].point,
                    direction: newDirection,
                    numBounces);
            }
        }
        return false;
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
            if (m_hits[i].collider.gameObject == gameObject)
                continue;

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