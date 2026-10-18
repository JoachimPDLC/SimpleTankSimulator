using System;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesManager
{
    private Dictionary<Teams, List<Tank>> m_entities = new();
    public Action m_onAllEnemiesKilled = null;
    public Action m_onAllPlayersKilled = null;

    public void ResetManager()
    {
        m_entities.Clear();
        for (int i = 0; i < (int)Teams.kCount; i++)        
            m_entities[(Teams)i] = new();
    }

    public Tank GetTarget(Vector3 position)
    {
        List<Tank> players = m_entities[Teams.kPlayers];

        if (players == null || players.Count == 0)
            return null;

        if (players.Count == 1)
            return players[0];

        Tank closestPlayer = null;
        float closestDistance = float.MaxValue;
        foreach (Tank player in players)
        {
            float distance = Vector3.Distance(position, 
                player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }
        return closestPlayer;
    }

    public void AddEntity(Teams team, Tank tank)
    {
        m_entities[team].Add(tank);
    }

    public void RemoveEntity(Teams team, Tank tank)
    {
        m_entities[team].Remove(tank);

        if (m_entities[team].Count == 0)
        {
            if (team == Teams.kPlayers)
                m_onAllPlayersKilled?.Invoke();
            else if (team == Teams.kEnemies)
                m_onAllEnemiesKilled?.Invoke();
        }
    }

    public void GetAllEnemies(out List<Tank> outList)
    {
        outList = m_entities[Teams.kEnemies];
    }
    
}
