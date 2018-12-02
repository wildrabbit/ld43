using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Entity: MonoBehaviour, IScheduledEntity
{
    public string Name;
    public Vector2Int Coords { get; protected set; }
    public InteractionType DefaultInteraction => _config.DefaultPlayerInteraction;
    public InteractionType DefaultMonsterInteraction => _config.MonsterInteraction;
    public int InteractionPriority => _config.Prio;

    public float HitChance { get { return _config.Stats.HitChance * 0.01f; } }
    public float AvoidChance { get { return _config.Stats.AvoidChance * 0.01f; } }
    public float CritChance { get { return _config.Stats.CritChance * 0.01f; } }
    public float CritDamageBonus { get { return _config.Stats.CritExtra * 0.01f; } }
    public float MinAttack { get { return _config.Stats.MinAttack; } }
    public float MaxAttack { get { return _config.Stats.MaxAttack; } }
    public float Defense { get { return _config.Stats.Defense; } }
    public float HP { get { return _hp; } }
    public float MaxHP { get { return _maxHP; } }

    public float HPPercent {  get { return _hp / _maxHP; } }
    protected float _hp;
    protected float _maxHP;
    protected Map _map;
    protected Vector2Int _startCoords;
    protected EntityConfig _config;

    protected StatsConfig _stats;

    protected IEntityController _entityController;
    protected MessageQueue _messageQueue;
    // TODO: Inventory

    // TODO: Effects

    public abstract void AddTime(float timeUnits, ref PlayContext playContext);

    public void Setup(EntityConfig entity, Map map, IEntityController controller, MessageQueue queue)
    {
        _map = map;
        _entityController = controller;
        _config = entity;
        _hp = _config.Stats.LifeData.HP;
        _maxHP = _config.Stats.LifeData.MaxHP;
        _messageQueue = queue;
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

    public bool TakeHit(float damageInflicted)
    {
        _hp -= damageInflicted;
        if(_hp <= 0)
        {
            _hp = 0;
            return true;
        }
        return false;
    }

    protected virtual void DoSetup()
    {

    }
}