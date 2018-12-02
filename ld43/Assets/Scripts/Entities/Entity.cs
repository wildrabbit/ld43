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

    public float HitChance { get { return (_stats[StatType.HitChance] + GetEquippedStat(StatType.HitChance)) * 0.01f; } }
    public float AvoidChance { get { return (_stats[StatType.AvoidChance] + GetEquippedStat(StatType.AvoidChance)) * 0.01f; } }
    public float CritChance { get { return (_stats[StatType.CritChance] + GetEquippedStat(StatType.CritChance)) * 0.01f; } }
    public float CritDamageBonus { get { return (_stats[StatType.CritDamage] + GetEquippedStat(StatType.CritDamage)) * 0.01f; } }
    public float MinAttack { get { return (_stats[StatType.MinAttack] + GetEquippedStat(StatType.MinAttack)); } }
    public float MaxAttack { get { return (_stats[StatType.MaxAttack] + GetEquippedStat(StatType.MaxAttack)); } }
    public float Defense { get { return (_stats[StatType.Defense] + GetEquippedStat(StatType.Defense)); } }
    public float HP { get { return _stats[StatType.HP]; } }
    public float HPRegen { get { return (_stats[StatType.HPRegen] + GetEquippedStat(StatType.HPRegen)) * 0.01f; } }
    public float MaxHP { get { return (_stats[StatType.MaxHP] + GetEquippedStat(StatType.MaxHP)) * (1f + 0.01f * GetEquippedStat(StatType.MaxHPPercent)); } }

    public float HPPercent {  get { return HP / MaxHP; } }

    protected Map _map;
    protected Vector2Int _startCoords;
    protected EntityConfig _config;

    protected IEntityController _entityController;
    protected MessageQueue _messageQueue;

    protected Dictionary<StatType, float> _stats;
   
    Inventory _inventory;

    public abstract void AddTime(float timeUnits, ref PlayContext playContext);

    public float GetEquippedStat(StatType statType)
    {
        if(_inventory.EquippedStats.TryGetValue(statType, out var value))
        {
            return value;
        }
        return 0.0f;
    }

    public void Setup(EntityConfig entity, Map map, IEntityController controller, MessageQueue queue)
    {
        _map = map;
        _entityController = controller;
        _config = entity;
        _messageQueue = queue;
        _inventory = new Inventory();
        _inventory.Setup(this);
        InitializeStats();
        DoSetup();
    }

    public void InitializeStats()
    {
        _stats = new Dictionary<StatType, float>();
        _stats.Add(StatType.MaxHP, _config.Stats.LifeData.MaxHP);
        _stats.Add(StatType.HP, _config.Stats.LifeData.HP);
        _stats.Add(StatType.HPRegen, _config.Stats.LifeData.HPRegen);
        _stats.Add(StatType.HitChance, _config.Stats.HitChance);
        _stats.Add(StatType.AvoidChance, _config.Stats.AvoidChance);
        _stats.Add(StatType.MinAttack, _config.Stats.MinAttack);
        _stats.Add(StatType.MaxAttack, _config.Stats.MaxAttack);
        _stats.Add(StatType.CritChance, _config.Stats.CritChance);
        _stats.Add(StatType.CritDamage, _config.Stats.CritExtra);
        _stats.Add(StatType.Defense, _config.Stats.Defense);

    }

    public bool TrySacrificeItem(ItemConfig item, float removalHPPercent, bool willRemove = false)
    {
        if(HPPercent < removalHPPercent)
        {
            return false;
        }
        if(!willRemove && _inventory.HasItem(item.ItemType))
        {
            return false;
        }
        _stats[StatType.MaxHP] = Mathf.Max(0, _stats[StatType.MaxHP] * (1 - removalHPPercent * 0.01f));
        _inventory.EquipItem(item, willRemove);
        return true;
    }

    public bool UsePotionItem()
    {
        if (_inventory.HasItem(ItemType.Potion))
        {
            _inventory.UseItem(ItemType.Potion);

            return true;
        }
        return false;
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
        _stats[StatType.HP] -= damageInflicted;
        if(_stats[StatType.HP] <= 0)
        {
            _stats[StatType.HP] = 0;
            return true;
        }
        return false;
    }


    protected void RegenHealth()
    {
        if (HP < MaxHP)
        {
            _stats[StatType.HP] = Mathf.Min(HP + HPRegen * MaxHP, MaxHP);
            if (Mathf.Approximately(HP, MaxHP))
            {
                _stats[StatType.HP] = MaxHP;
                _messageQueue.AddEntry(Name + "'s HP restored to Max(" + MaxHP + ")");
            }
        }
    }


    public void AddPermanentEffects(List<Effect> effects)
    {
        foreach (var effect in effects)
        {
            _stats[effect.AffectedStat] = effect.Value;
        }
    }


    protected virtual void DoSetup()
    {

    }

    public virtual PlayContext PlayerInteracts()
    {
        return PlayContext.Action;
    }
}