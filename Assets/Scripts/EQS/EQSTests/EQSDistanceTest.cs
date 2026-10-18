using EQS;
using UnityEngine;

[CreateAssetMenu(fileName = "EQSDistanceTest", menuName = "Scriptable Objects/EQSDistanceTest")]
public class EQSDistanceTest : EQSTest
{
    [SerializeField] float kDecayValue = 5f;
    [SerializeField] bool m_checkQuerier = false;
    [SerializeField] bool m_checkFar = false;

    public override float Score(ref EQSQueryPoint point, in EQSContext context)
    {
        Vector3 targetPos = context.Target.position;
        if (m_checkQuerier)
            targetPos = context.Querier.position;

        float distance = Vector3.Distance(point.Position, targetPos);
        float score = Mathf.Exp(-distance / kDecayValue);
        if (m_checkFar)
            return 1 - score;
        else
            return score;
    }
}
