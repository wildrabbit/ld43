using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterConfig", menuName = "Sacrificelike/MonsterConfig")]
public class MonsterConfig: EntityConfig
{
    public float ThinkingDelay = 1.4f;
    public float PathUpdateDelay = 6.0f;
    public float EscapeHPPercent = 25.0f;
    public float KeepAttackingPlayerIfBelowHPPercent = 20.0f;
}
