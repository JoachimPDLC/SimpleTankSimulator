using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public class SaveFile
    {
        public List<int> CompletedLevels = new();
    }

    private static readonly string kFileName = "Save.sav";

    public static void Save(SaveFile save)
    {
        DataSaver.Save(save, kFileName);
    }

    public static SaveFile Load()
    {
        return DataSaver.Load<SaveFile>(kFileName);
    }

    public static bool DoesSaveExist()
    {
        return DataSaver.DoesFileExist(kFileName);
    }

    public static void DeleteSave()
    {
        DataSaver.DeleteFile(kFileName);
    }
}
