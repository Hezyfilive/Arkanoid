using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text bestScore;
    public TMP_Text scoreText;
    public TMP_Text livesText;
    private int _bestScore;
    private int _level = 1;
    private int Score { get; set; }


    private void Start()
    {
        Brick.OnBrickDestruction += OnBrickDestruction;
        Generator.OnLevelLoad += OnLevelLoad;
        GameManager.Instance.OnLiveLost += OnLiveLost;
        GameManager.Instance.OnDeath += OnDeath;
        Score = 0;
    }

    private void OnDisable()
    {
        Brick.OnBrickDestruction -= OnBrickDestruction;
        Generator.OnLevelLoad -= OnLevelLoad;
    }

    private void OnLevelLoad()
    {
        _level++;
    }

    private void OnBrickDestruction(Brick brick)
    {
        UpdateScoreText(brick.maxHitPoints * _level);
    }

    private void OnLiveLost(int lives)
    {
        livesText.text = $@"Lives: <br>{lives}";
        _level--;
    }

    private void UpdateScoreText(int increment)
    {
        Score += increment;
        var scoreString = Score.ToString().PadLeft(5, '0');
        scoreText.text = $@"Score: <br>{scoreString}";
    }

    private void OnDeath()
    {
        _level = 0;
        _bestScore = Score > _bestScore ? Score : _bestScore;
        var scoreString = _bestScore.ToString().PadLeft(5, '0');
        bestScore.text = $@"Best: <br>{scoreString}";
        Score = 0;
        livesText.text = @"Lives: <br>3";
        scoreText.text = @"Score: <br>00000";
    }

    #region Singleton

    public static UIManager Instance { get; private set; }

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