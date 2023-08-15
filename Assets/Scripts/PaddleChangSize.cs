public class PaddleChangSize : Collectable
{
    public float newWidth = 2;

    protected override void ApplyEffect()
    {
        if (Paddle.Instance != null && !Paddle.Instance.paddleIsTransforming)
            Paddle.Instance.StartWithAnimation(newWidth);
    }
}