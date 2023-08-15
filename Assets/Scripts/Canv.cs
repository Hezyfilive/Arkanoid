using UnityEngine;

public class Canv : MonoBehaviour
{
    #region Singleton

    public static Canv Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    #endregion
}