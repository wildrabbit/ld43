using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class StatsConfig
{
    public LifeConfig LifeData;

    public float HitChance;
    public float AvoidChance;
    public float MinAttack;
    public float MaxAttack;
    public float Defense;
    public float CritChance;
    public float CritExtra;

    [FormerlySerializedAs("Range")]
    public int BaseAttackRange;
    public int VisibilityRange;
}

[Serializable]
public class LifeConfig
{
    public float MaxHP;
    public float HP;
    public float HPRegen;
}
