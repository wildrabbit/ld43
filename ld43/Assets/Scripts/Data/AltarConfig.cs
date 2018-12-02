using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AltarConfig", menuName = "Sacrificelike/AltarConfig")]
public class AltarConfig : EntityConfig
{
    public float DefaultMaxHPPercentCost = 25;
    public float RemovalMaxHPPercentCost = 50;

    public List<ItemConfig> AvailableLoot;
}
