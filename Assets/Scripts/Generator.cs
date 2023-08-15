using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    public GameObject[] bricks;

    public int mapWidght = 16;

    public int mapHight = 8;

    public float magnification = 7.0f;
    public int xOffset;
    public int yOffset;
    public float seed;

    private readonly List<List<int>> _noiseGrid = new();
    private readonly List<List<GameObject>> _tileGrid = new();

    private Dictionary<int, GameObject> _tileGroup;
    private Dictionary<int, GameObject> _tileSet;

    private void Start()
    {
        seed = Random.Range(-4000000.0f, 999999.3f);
        CreateTileSet();
        CreateTileGroup();
        GenerateMap();
    }

    public static event Action OnLevelLoad;

    private void CreateTileSet()
    {
        _tileSet = new Dictionary<int, GameObject>();
        var count = 0;
        foreach (var brick in bricks)
        {
            _tileSet.Add(count, brick);
            count++;
        }
    }

    private void CreateTileGroup()
    {
        _tileGroup = new Dictionary<int, GameObject>();
        foreach (var prefabPair in _tileSet)
        {
            var tileGroup = new GameObject(prefabPair.Value.name)
            {
                transform =
                {
                    parent = gameObject.transform,
                    localPosition = new Vector3(0, 0, 0)
                }
            };
            _tileGroup.Add(prefabPair.Key, tileGroup);
        }
    }

    private void GenerateMap()
    {
        for (var x = 0; x < mapWidght; x++)
        {
            _noiseGrid.Add(new List<int>());
            _tileGrid.Add(new List<GameObject>());
            for (var y = 0; y < mapHight; y++)
            {
                var tileId = GetIdUsingPerlin(x, y);
                _noiseGrid[x].Add(tileId);
                CreateTile(tileId, x, y);
            }
        }
        
        OnLevelLoad?.Invoke();
    }

    private int GetIdUsingPerlin(int x, int y)
    {
        var rawPerlin = Mathf.PerlinNoise(
            (x + seed - xOffset) / magnification,
            (y + seed - yOffset) / magnification
        );
        var clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);
        var scalePerlin = clampPerlin * (_tileSet.Count + 1);
        if (scalePerlin >= 4) scalePerlin = 3;
        return Mathf.FloorToInt(scalePerlin);
    }

    private void CreateTile(int tileId, int x, int y)
    {
        if (tileId <= 0) return;
        var tilePrefab = _tileSet[tileId - 1];
        var tileGroup = _tileGroup[tileId - 1];
        var tile = Instantiate(tilePrefab, tileGroup.transform);

        tile.name = $"tile_x{x}_y{y}";
        tile.transform.localPosition = new Vector3(x * 0.4f, y * 0.4f, 0);
        _tileGrid[x].Add(tile);
    }


    public void RegenerateLevel()
    {
        for (var i = 0; i < gameObject.transform.childCount; i++) Destroy(gameObject.transform.GetChild(i).gameObject);
        _tileGrid.Clear();
        _noiseGrid.Clear();
        _tileGroup.Clear();
        _tileSet.Clear();
        seed = Random.Range(-4000000.0f, 999999.3f);
        CreateTileSet();
        CreateTileGroup();
        GenerateMap();
    }

    public void RestartLevel()
    {
        for (var i = 0; i < gameObject.transform.childCount; i++) Destroy(gameObject.transform.GetChild(i).gameObject);
        _tileGrid.Clear();
        _noiseGrid.Clear();
        _tileGroup.Clear();
        _tileSet.Clear();
        CreateTileSet();
        CreateTileGroup();
        GenerateMap();
    }
}