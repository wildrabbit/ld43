using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Boots,
    Ring,
    Potion
}

public enum StatType
{
    HPRegen,
    MaxHP,
    MaxHPPercent,
    HP,
    HPPercent,
    Attack,
    MinAttack,
    MaxAttack,
    Defense,
    HitChance,
    AvoidChance,
    CritChance,
    CritDamage,
    Speed,
    SpeedPercent
}

[System.Serializable]
public class Effect
{
    public StatType AffectedStat;
    public float Value;
}

[CreateAssetMenu(fileName = "ItemConfig", menuName = "Sacrificelike/ItemConfig")]
public class ItemConfig: ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public string Desc;
    public ItemType ItemType;
    public List<Effect> Effects;
}
