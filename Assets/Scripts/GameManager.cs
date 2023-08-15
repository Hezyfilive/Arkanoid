using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Map;
    public int availableLives = 3;
    private List<int> _bricks = new() { 1, 2, 3 };
    public bool IsGameStarted { get; set; }
    private int Lives { get; set; }

    private void Start()
    {
        Brick.OnBrickDestruction += OnOnBrickDestruction;
        Screen.SetResolution(540, 940, false);
        Ball.OnBallDeath += OnBallDeath;
        Generator.OnLevelLoad += OnLevelLoad;
        Lives = availableLives;
    }

    private void OnDisable()
    {
        Brick.OnBrickDestruction -= OnOnBrickDestruction;
        Ball.OnBallDeath -= OnBallDeath;
    }

    public event Action<int> OnLiveLost;
    public event Action OnDeath;

    private void OnLevelLoad()
    {
        _bricks = new List<int>{ 1, 2, 3 };
        for (var x = 0; x < 3; x++) _bricks[x] = Map.transform.GetChild(x).childCount;
    }

    private void OnOnBrickDestruction(Brick obj)
    {
        var objectName = obj.transform.parent.gameObject.name;
        switch (objectName)
        {
            case "Brick":
                _bricks[0]--;
                _bricks[0] = _bricks[0] < 0 ? 0 : _bricks[0];
                Debug.Log(_bricks[0]);
                break;
            case "Brick 2":
                _bricks[1]--;
                _bricks[1] = _bricks[1] < 0 ? 0 : _bricks[1];
                Debug.Log(_bricks[1]);
                break;
            default:
                _bricks[2]--;
                _bricks[2] = _bricks[2] < 0 ? 0 : _bricks[2];
                Debug.Log(_bricks[2]);
                break;
        }

        if (_bricks.All(o => o == 0))
        {
            BallManager.Instance.ResetBall();
            Map.GetComponent<Generator>().RegenerateLevel();
            IsGameStarted = false;
            return;
        }

        StartCoroutine(RecalculateBricks());
    }

    private void OnBallDeath(Ball ball)
    {
        if (BallManager.Instance.Balls.Count > 0) return;
        Lives--;
        if (Lives < 1)
        {
            OnDeath?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            BallManager.Instance.ResetBall();
            IsGameStarted = false;
            Map.GetComponent<Generator>().RestartLevel();
            OnLiveLost?.Invoke(Lives);
        }
    }
    
    private IEnumerator RecalculateBricks()
    {
        yield return new WaitForSeconds(1);
        for (var x = 0; x < 3; x++) _bricks[x] = Map.transform.GetChild(x).childCount;
        if (_bricks.All(o => o == 0))
        {
            BallManager.Instance.ResetBall();
            Map.GetComponent<Generator>().RegenerateLevel();
            IsGameStarted = false;
        }
        
    }
    #region Singleton

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    #endregion
}