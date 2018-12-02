using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Inventory
{
    Entity _owner;
    Dictionary<ItemType, ItemConfig> _inventorySlots;
    Dictionary<StatType, float> _equippedStats;

    public Dictionary<StatType, float> EquippedStats
    {
        get
        {
            return _equippedStats;
        }
    }

    public void Setup(Entity owner)
    {
        _owner = owner;
        _inventorySlots = new Dictionary<ItemType, ItemConfig>();
        foreach (var itemType in System.Enum.GetValues(typeof(ItemType)))
        {
            _inventorySlots.Add((ItemType)itemType, null);
        }
        _equippedStats = new Dictionary<StatType, float>();
    }

    public Dictionary<StatType, float> GetCombinedEffects()
    {
        return _equippedStats;
    }

    public bool HasItem(ItemType type)
    {
        return _inventorySlots.ContainsKey(type) && _inventorySlots[type] == null;
    }

    public ItemConfig GetEquippedItem(ItemType slot)
    {
        if(_inventorySlots.TryGetValue(slot, out var config))
        {
            return config;
        }
        return null;
    }

    public bool RemoveItem(ItemType slot)
    {
        if (_inventorySlots.TryGetValue(slot, out var config) && config != null)
        {
            _inventorySlots[slot] = null;
            foreach (var effect in config.Effects)
            {
                _equippedStats[effect.AffectedStat] -= effect.Value;
            }
            return true;
        }
        return false;
    }

    public bool EquipItem(ItemConfig config, bool forced = false)
    {
        if(_inventorySlots.ContainsKey(config.ItemType) && (forced || _inventorySlots[config.ItemType] == null))
        {
            ItemConfig oldItem = _inventorySlots[config.ItemType];
            if(oldItem != null)
            {
                foreach (var effect in oldItem.Effects)
                {
                    _equippedStats[effect.AffectedStat] -= effect.Value;
                }
            }
            _inventorySlots[config.ItemType] = config;
            foreach (var effect in config.Effects)
            {
                _equippedStats[effect.AffectedStat] -= effect.Value;
            }
        }
        return false;
    }

    public bool UseItem(ItemType slot)
    {
        if(slot != ItemType.Potion || !HasItem(slot))
        {
            return false;
        }

        _owner.AddPermanentEffects(_inventorySlots[slot].Effects);

        _inventorySlots[slot] = null;
        return true;
    }
}
