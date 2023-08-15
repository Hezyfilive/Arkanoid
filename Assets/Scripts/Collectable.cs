using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Padle"))
        {
            ApplyEffect();
            Destroy(gameObject);
        }

        if (other.CompareTag("DeathWall")) Destroy(gameObject);
    }

    protected abstract void ApplyEffect();
}