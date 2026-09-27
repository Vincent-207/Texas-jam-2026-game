using UnityEngine;

public class MusicSingleton : MonoBehaviour
{
    public static MusicSingleton instance;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

    }
}
