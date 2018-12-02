using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Map: MonoBehaviour, IScheduledEntity
{
    public class SpawnData
    {
        public MonsterSpawnPoint Point;
        public float Delay;
        public float Elapsed;
        public SpawnData(MonsterSpawnPoint point, float delay)
        {
            Point = point;
            Delay = delay;
            Elapsed = 0.0f;
        }
    }

    Tilemap _groundTilemap;
    Tilemap _wallsTilemap;
    Vector2Int _playerStart;

    float _elapsed;

    Dictionary<Vector2Int, SpawnData> _monsterSpawnCoords;

    IEntityController _entityController;
    
    public Vector2Int PlayerStartCoords => _playerStart;
    public float Speed => throw new System.NotImplementedException();

    public void Setup(IEntityController entityController)
    {
        _entityController = entityController;
        SetupPlayerStart();
        SetupMonsterStartCoords();
    }

    public void AddTime(float timeUnits, ref PlayContext playContext)
    {
        _elapsed += timeUnits;

        if(_entityController.ReachedMonsterLimit)
        {
            return;
        }

        foreach(var pair in _monsterSpawnCoords)
        {
            pair.Value.Elapsed += timeUnits;
            if (pair.Value.Elapsed >= pair.Value.Delay && !_entityController.FindEntityNearby(pair.Key, 1))
            {
                pair.Value.Delay = Random.Range(pair.Value.Point.MinSpawnTime, pair.Value.Point.MaxSpawnTime);
                pair.Value.Elapsed = 0;
                MonsterConfig cfg = PickMonster(pair.Value.Point.MonsterEntries);
                if(cfg != null)
                {
                    // Add some small noise to the coords.
                    Vector2Int coords = pair.Key;
                    coords.x += Random.Range(-1, 2);
                    coords.y += Random.Range(-1, 2);
                    _entityController.CreateMonster(cfg, coords);
                }
            }
        }
    }

    MonsterConfig PickMonster(List<MonsterSpawnPoint.MonsterSpawnEntry> entries)
    {
        MonsterConfig result = null;
        int sumWeights = 0;
        foreach(var entry in entries)
        {
            sumWeights += entry.weight;
        }

        int randomPick = Random.Range(0, sumWeights);
        int aggregate = 0;
        foreach (var entry in entries)
        {
            aggregate += entry.weight;
            if(aggregate > randomPick)
            {
                result = entry.Config;
                break;
            }
        }
        
        return result;
    }

    public Vector3 WorldFromGrid(Vector2Int coords)
    {
        return WorldFromGrid(new Vector3Int(coords.x, coords.y, 0));
    }

    public Vector3 WorldFromGrid(Vector3Int coords)
    {
        return _groundTilemap.layoutGrid.GetCellCenterWorld(coords);
    }

    void SetupPlayerStart()
    {
        Transform playerStart = transform.Find("PlayerStart");
        if(playerStart)
        {
            _playerStart = (Vector2Int)_groundTilemap.layoutGrid.WorldToCell(playerStart.position);
        }
    }

    public bool IsWalkable(Vector2Int coords)
    {
        TileBase tile = _groundTilemap.GetTile((Vector3Int)coords);
        if(tile == null)
        {
            return false;
        }
        return true;
    }

    void SetupMonsterStartCoords()
    {
        GameObject[] monsterSpawns = GameObject.FindGameObjectsWithTag("Spawn");
        foreach(var spawn in monsterSpawns)
        {
            MonsterSpawnPoint spawnPoint = spawn.GetComponent<MonsterSpawnPoint>();
            Vector2Int coords = (Vector2Int)_groundTilemap.layoutGrid.WorldToCell(spawn.transform.position);
            float initialDelay = Random.Range(spawnPoint.MinSpawnTime, spawnPoint.MaxSpawnTime);
            _monsterSpawnCoords.Add(coords, new SpawnData(spawnPoint,initialDelay));
        }
    }

    void Awake()
    {
        _groundTilemap = transform.Find("Ground").GetComponent<Tilemap>();
        _wallsTilemap = transform.Find("Walls").GetComponent<Tilemap>();
        _monsterSpawnCoords = new Dictionary<Vector2Int, SpawnData>();
    }
}