using System.Collections;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Camera mainCamera;
    public float defaultPaddleWidth;

    public SpriteRenderer sr;
    private float _paddleInitialY;

    private void Start()
    {
        _paddleInitialY = transform.position.y;
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        PaddleMovement();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Ball")) return;
        var ballRb = other.gameObject.GetComponent<Rigidbody2D>();
        Vector3 hitPoint = other.contacts[0].point;
        var position = gameObject.transform.position;
        var paddleCenter = new Vector3(position.x, position.y, 0);

        ballRb.velocity = Vector2.zero;

        var difference = paddleCenter.x - hitPoint.x;
        ballRb.AddForce(hitPoint.x < paddleCenter.x
            ? new Vector2(-Mathf.Abs(difference * 200), BallManager.Instance.initialBallSpeed)
            : new Vector2(Mathf.Abs(difference * 200), BallManager.Instance.initialBallSpeed));
    }

    private void PaddleMovement()
    {
        var paddleShift = defaultPaddleWidth - defaultPaddleWidth / 2 * sr.size.x;
        var leftClamp = 135 - paddleShift;
        var rightClamp = 410 + paddleShift;
        var mousePosition = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        var mousePositionWorld = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition, 0, 0)).x;
        transform.position = new Vector3(mousePositionWorld, _paddleInitialY, 0);
    }

    #region Singleton

    public static Paddle Instance { get; private set; }
    public bool paddleIsTransforming;
    public float changeDuration = 10;
    public float paddleWidth = 2;
    public float paddleHeight = 0.28f;
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    #endregion

    // ReSharper disable Unity.PerformanceAnalysis
    public void StartWithAnimation(float newWidth)
    {
        StartCoroutine(AnimatePaddleWidth(newWidth));
    }

    private IEnumerator AnimatePaddleWidth(float width)
    {
        paddleIsTransforming = true;
        StartCoroutine(ResetPaddleWidthAfterTime(changeDuration));
        if (width > sr.size.x)
        {
            var currentWidth = sr.size.x;
            while (currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2;
                sr.size = new Vector2(currentWidth, paddleHeight);
                _boxCollider2D.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        else
        {
            var currentWidth = sr.size.x;
            while (currentWidth > width)
            {
                currentWidth -= Time.deltaTime * 2;
                sr.size = new Vector2(currentWidth, paddleHeight);
                _boxCollider2D.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }

        paddleIsTransforming = false;
    }

    private IEnumerator ResetPaddleWidthAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartWithAnimation(paddleWidth);
    }
}