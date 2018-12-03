using System;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

public class Altar : Entity
{
    AltarConfig _altarConfig;

    public AltarConfig Config => _altarConfig;

    public bool Used;

    public event Action<Altar, Action> PlayerInteracted;

    protected override void DoSetup()
    {
        _altarConfig = _config as AltarConfig;
        Used = false;
    }

    public override PlayContext PlayerInteracts()
    {
        PlayerInteracted?.Invoke(this, () => Used = true);
        return PlayContext.AltarContext;
    }

    public override void AddTime(float timeUnits, ref PlayContext playContext)
    {
    }
}