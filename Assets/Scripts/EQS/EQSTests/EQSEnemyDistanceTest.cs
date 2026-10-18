using UnityEngine;
using EQS;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EQSEnemyDistanceTest", menuName = "Scriptable Objects/EQSEnemyDistanceTest")]

public class EQSEnemyDistanceTest : EQSTest
{
    [SerializeField] float kDecayValue = 5f;
    [SerializeField] bool m_checkFar = false;

    public override float Score(ref EQSQueryPoint point, in EQSContext context)
    {
        List<Tank> enemies = new();
        Level.GetEntitiesManager().GetAllEnemies(out enemies);

        if (enemies.Count <= 1)
            return 1;

        float closestDistance = float.MaxValue;

        foreach (Tank enemy in enemies)
        {
            Transform enemyTransform = enemy.transform;

            if (context.Querier == enemyTransform)
                continue;

            float distance = Vector3.Distance(point.Position,
                enemyTransform.position);

            if (distance < closestDistance)
                closestDistance = distance;
        }
        
        float score = Mathf.Exp(-closestDistance / kDecayValue);
        if (m_checkFar)
            return 1 - score;
        else
            return score;
    }
}
