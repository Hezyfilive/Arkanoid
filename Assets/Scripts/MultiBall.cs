using System.Linq;

public class MultiBall : Collectable
{
    protected override void ApplyEffect()
    {
        foreach (var ball in BallManager.Instance.Balls.ToList())
            BallManager.Instance.SpawnBalls(ball.gameObject.transform.position, 2);
    }
}