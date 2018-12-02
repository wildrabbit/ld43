using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatsConfig
{
    public LifeConfig LifeData;

    public float Speed;
    public float HitChance;
    public float AvoidChance;
    public float MinAttack;
    public float MaxAttack;
    public float Defense;
    public float CritChance;
    public float CritExtra;
    public int Range;
    public int VisibilityRange;
}

[Serializable]
public class LifeConfig
{
    public float MaxHP;
    public float HP;
    public float HPRegen;
}
