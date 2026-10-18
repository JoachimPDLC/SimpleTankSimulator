using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    static private HashSet<int> m_completionTracker = new();
    static private SaveManager.SaveFile m_saveFile = new();

    public static void AddLevelCompletion(string sceneName)
    {
        m_completionTracker.Add(sceneName.GetHashCode());
    }

    public static bool IsLevelCompleted(string sceneName)
    {
        return m_completionTracker.Contains(sceneName.GetHashCode());
    }

    public static void Save()
    {
        m_saveFile.CompletedLevels.Clear();
        foreach (int level in m_completionTracker)        
            m_saveFile.CompletedLevels.Add(level);
        SaveManager.Save(m_saveFile);
    }

    public static void Load()
    {
        if (!SaveManager.DoesSaveExist())
            return;

        m_saveFile = SaveManager.Load();
        m_completionTracker.Clear();        
        foreach (int level in m_saveFile.CompletedLevels)
            m_completionTracker.Add(level);
    }

    public static void ResetCompletion()
    {
        m_completionTracker.Clear();
        SaveManager.DeleteSave();
    }

    public static bool DoesSaveExist()
    {
        return SaveManager.DoesSaveExist();
    }
}
