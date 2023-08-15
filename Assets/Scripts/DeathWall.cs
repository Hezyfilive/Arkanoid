using UnityEngine;

public class DeathWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;
        var ball = other.GetComponent<Ball>();
        BallManager.Instance.Balls.Remove(ball);
        ball.Die();
    }
}