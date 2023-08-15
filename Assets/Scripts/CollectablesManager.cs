using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    public List<Collectable> availableBuffs;
    public List<Collectable> availableDebuffs;
    [Range(0, 100)] public float buffChance;
    [Range(0, 100)] public float debuffChance;

    public void Start()
    {
        Brick.OnBrickDestruction += OnBrickDestroyed;
    }

    public void OnDisable()
    {
        Brick.OnBrickDestruction -= OnBrickDestroyed;
    }

    private void OnBrickDestroyed(Brick brick)
    {
        var buffSpawnChance = Random.Range(0, 100f);
        var debuffSpawnChance = Random.Range(0, 100f);
        Collectable prefab = null;

        var alreadySpawned = false;

        if (buffSpawnChance <= buffChance)
        {
            alreadySpawned = true;
            var index = Random.Range(0, availableBuffs.Count);
            prefab = availableBuffs[index];
        }

        if (debuffSpawnChance <= debuffChance && !alreadySpawned)
        {
            var index = Random.Range(0, availableDebuffs.Count);
            prefab = availableDebuffs[index];
        }

        if (prefab != null) Instantiate(prefab, brick.gameObject.transform.position, Quaternion.identity);
    }

    #region Singleton

    public static CollectablesManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    #endregion
}