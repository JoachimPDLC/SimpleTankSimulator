using UnityEngine;

public class MonoMusicManager : MonoBehaviour
{
    private static MonoMusicManager s_instance = null;

    private void Awake()
    {
        if (s_instance != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(this);
            s_instance = this;
        }
    }
}
