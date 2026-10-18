using System.IO;
using System.Text;
using UnityEngine;

public class DataSaver
{
    static private readonly string kDataPath = Application.persistentDataPath;

    public static void Save<T>(T data, string fileName)
    {
        string path = Path.Combine(kDataPath, fileName);

        string jsonText = JsonUtility.ToJson(data);
        byte[] jsonByte = Encoding.ASCII.GetBytes(jsonText);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
            Directory.CreateDirectory(Path.GetDirectoryName(path));

        File.WriteAllBytes(path, jsonByte);
    }

    public static T Load<T>(string fileName)
    {
        string path = Path.Combine(kDataPath, fileName);

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
            return default;

        if (!File.Exists(path))
            return default;            

        //Load saved Json
        byte[] jsonByte = null;
        jsonByte = File.ReadAllBytes(path);
        string jsonData = Encoding.ASCII.GetString(jsonByte);
        T result = JsonUtility.FromJson<T>(jsonData);
        return result;
    }

    public static bool DoesFileExist(string fileName)
    {
        string path = Path.Combine(kDataPath, fileName);
        return File.Exists(path);
    }

    public static void DeleteFile(string fileName)
    {
        string path = Path.Combine(kDataPath, fileName);
        if (File.Exists(path))        
            File.Delete(path);        
    }
}
