#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneRef
{
#if UNITY_EDITOR
    public SceneAsset Scene = null;
#endif

    public string Name = "";
}
