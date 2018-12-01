using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Map: MonoBehaviour, IScheduledEntity
{
    Tilemap _groundTilemap;
    Tilemap _wallsTilemap;
    Vector2Int _playerStart;
    

    public Vector2Int PlayerStartCoords => _playerStart;
    public float Speed => throw new System.NotImplementedException();

    public void AddTime(float timeUnits, ref PlayContext playContext)
    {
        
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

    void Awake()
    {
        _groundTilemap = transform.Find("Ground").GetComponent<Tilemap>();
        _wallsTilemap = transform.Find("Walls").GetComponent<Tilemap>();
    }
}