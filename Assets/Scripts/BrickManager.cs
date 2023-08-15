using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public Sprite[] sprites;

    #region Singleton

    public static BrickManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    #endregion
}