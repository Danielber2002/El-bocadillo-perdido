using UnityEngine;

public class GlovalMusicSystem : MonoBehaviour
{
    private static GlovalMusicSystem instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
