using System.Collections.Generic;
using UnityEngine;

namespace EQS
{
    public class EQS : MonoBehaviour
    {
        public Transform m_target;
        [SerializeField] private Transform m_ownerTransform;

        [SerializeField] private float m_querySize;
        [SerializeField] private uint m_numDivisions;
        [SerializeField] private EQSTest[] m_tests;

        private EQSContext m_context;

        [Header("Gizmos")]
        [SerializeField] private float kGizmoSphereRadius;
        [SerializeField] private Color kHighestScoreColor;
        [SerializeField] private Color kLowestScoreColor;
        [SerializeField] private Color kInvalidPointColor;

        private EQSResult m_result;

        private void Start()
        {
            if (m_target != null)
                m_context = new EQSContext(m_ownerTransform, m_target);
        }

        private void OnDrawGizmosSelected()
        {
            if (m_result.AllPoints == null ||
                m_result.AllPoints.Count <= 0 ||
                m_target == null)
                return;

            if (m_context == default)
                m_context = new EQSContext(m_ownerTransform, m_target);

            float scoresRange = m_result.HighestScore - m_result.LowestScore;

            for (int i = 0; i < m_result.AllPoints.Count; i++)
            {
                if (!m_result.AllPoints[i].Valid)
                {
                    Gizmos.color = kInvalidPointColor;
                    Gizmos.DrawWireSphere(
                        m_result.AllPoints[i].Position,
                        kGizmoSphereRadius);
                    continue;
                }

                if (i == m_result.ChosenPointIndex)
                {
                    Gizmos.color = kHighestScoreColor;
                    Gizmos.DrawWireSphere(
                        m_result.AllPoints[i].Position,
                        kGizmoSphereRadius * 2);
                    continue;
                }

                float score = m_result.AllPoints[i].Score;
                float normalizedScore = 
                    (score - m_result.LowestScore) / scoresRange;                

                Gizmos.color = Color.Lerp(kLowestScoreColor,
                    kHighestScoreColor, normalizedScore);
                Gizmos.DrawWireSphere(
                    m_result.AllPoints[i].Position,
                    kGizmoSphereRadius);
            }
        }

        public void SetTarget(Transform targetTransform)
        {
            m_target = targetTransform;
            m_context = new EQSContext(m_ownerTransform, m_target);
        }

        public EQSResult Query()
        {
            if (m_target == null)
                return default;

            if (m_context == default)
                m_context = new EQSContext(m_ownerTransform, m_target);

            m_result = new EQSResult();
            List<EQSQueryPoint> points = CreatePoints();
            int bestPointIndex = int.MaxValue;
            float bestScore = float.MinValue;
            float lowestScore = float.MaxValue;
            for (int i = 0; i < points.Count; i++)
            {
                EQSQueryPoint point = points[i];
                float pointScore = 1;

                foreach (EQSTest test in m_tests)
                {
                    if (!point.Valid)
                        break;
                    
                    pointScore *= test.Score(ref point, m_context);
                    if (pointScore <= 0)
                        break;
                }

                point.Score = pointScore;
                points[i] = point;

                if (!points[i].Valid)
                    continue;

                if (points[i].Score < lowestScore)
                    lowestScore = points[i].Score;

                if (points[i].Score > bestScore)
                {
                    bestScore = points[i].Score;
                    bestPointIndex = i;
                }
            }
            m_result.AllPoints = points;
            m_result.LowestScore = lowestScore;
            m_result.HighestScore = bestScore;

            if (bestPointIndex == int.MaxValue)
            {
                m_result.Success = false;
                return m_result;
            }

            m_result.Success = true;
            m_result.Position = points[bestPointIndex].Position;
            m_result.ChosenPointIndex = bestPointIndex;
            return m_result;
        }

        private List<EQSQueryPoint> CreatePoints()
        {
            int count = (int)m_numDivisions * (int)m_numDivisions;
            List<EQSQueryPoint> result = new List<EQSQueryPoint>(count);
            float step = m_querySize / m_numDivisions;

            Vector3 ownerPos = m_ownerTransform.position;
            float startX = ownerPos.x -m_querySize / 2;
            float startZ = ownerPos.z -m_querySize / 2;

            for (int x = 0; x < m_numDivisions; x++)
            {
                for (int z = 0; z < m_numDivisions; z++)
                {
                    float pointX = startX + x * step + step * 0.5f;
                    float pointZ = startZ + z * step + step * 0.5f;

                    result.Add(new EQSQueryPoint(new(pointX, ownerPos.y, pointZ)));
                }
            }

            return result;
        }
    }
}
