using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int maxHitPoints = 1;
    public ParticleSystem destroyEffect;
    private int _hitPoints;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _hitPoints = maxHitPoints;
        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = BrickManager.Instance.sprites[_hitPoints - 1];
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CollisionLogic();
    }

    public static event Action<Brick> OnBrickDestruction;

    private void CollisionLogic()
    {
        _hitPoints--;
        if (_hitPoints <= 0)
        {
            OnBrickDestruction?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(gameObject);
        }
        else
        {
            _sr.sprite = BrickManager.Instance.sprites[_hitPoints - 1];
        }
    }

    private void SpawnDestroyEffect()
    {
        var brickPos = gameObject.transform.position;
        var spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z - .2f);
        var effect = Instantiate(destroyEffect.gameObject, spawnPosition, Quaternion.identity);
        var mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = _sr.color;
        Destroy(effect, destroyEffect.main.startLifetime.constant);
    }
}