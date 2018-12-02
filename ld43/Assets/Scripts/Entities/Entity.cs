using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity: MonoBehaviour, IScheduledEntity
{
    public Vector2Int Coords { get; protected set; }
    public float Speed => _config.Stats.Speed;

    protected float _hp;
    protected Map _map;
    protected Vector2Int _startCoords;
    protected Vector2Int _coords;
    protected EntityConfig _config;

    protected StatsConfig _stats;

    // TODO: Inventory

    // TODO: Effects

    public abstract void AddTime(float timeUnits, ref PlayContext playContext);

    public void Setup(EntityConfig entity, Map map)
    {
        _map = map;
        _config = entity;
        DoSetup();
    }

    public void SetGridPos(Vector2Int pos)
    {
        Coords = pos;
        transform.position = _map.WorldFromGrid(pos);
    }

    public void StartGame(Vector2Int startCoords)
    {
        SetGridPos(startCoords);
    }

    protected virtual void DoSetup()
    {

    }
}