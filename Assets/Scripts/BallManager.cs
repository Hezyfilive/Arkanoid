using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;

    public float initialBallSpeed = 250;

    private Rigidbody2D initalBallRb;
    private Ball initialBall;
    public List<Ball> Balls { get; set; }

    private void Start()
    {
        InitBall();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameStarted) return;
        var paddlePosition = Paddle.Instance.gameObject.transform.position;
        var ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
        initialBall.transform.position = ballPosition;

        if (!Input.GetMouseButtonDown(0)) return;
        initalBallRb.isKinematic = false;
        initalBallRb.AddForce(new Vector3(0, initialBallSpeed));
        GameManager.Instance.IsGameStarted = true;
    }

    private void InitBall()
    {
        var paddlePosition = Paddle.Instance.gameObject.transform.position;
        var startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
        initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        initalBallRb = initialBall.GetComponent<Rigidbody2D>();

        Balls = new List<Ball>
        {
            initialBall
        };
    }

    public void ResetBall()
    {
        foreach (var ball in Balls.ToList()) Destroy(ball.gameObject);

        InitBall();
    }

    public void SpawnBalls(Vector3 position, int count)
    {
        for (var x = 0; x < count; x++)
        {
            var spawnedBall = Instantiate(ballPrefab, position, Quaternion.identity);
            var spawnedBallRb = spawnedBall.GetComponent<Rigidbody2D>();
            spawnedBallRb.isKinematic = false;
            spawnedBallRb.AddForce(new Vector2(0, initialBallSpeed));
            Balls.Add(spawnedBall);
        }
    }

    #region Singleton

    public static BallManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    #endregion
}