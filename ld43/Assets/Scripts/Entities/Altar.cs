using System;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

public class Altar : Entity
{
    AltarConfig _altarConfig;
    bool Used;

    protected override void DoSetup()
    {
        _altarConfig = _config as AltarConfig;
        Used = false;
    }

    public override PlayContext PlayerInteracts()
    {
        //Action callback = Used ? null : 
        //_entityController.SetupAltarContext(_altarConfig, sacrificeAvailable:  Used, callback);
        return PlayContext.AltarContext;
    }

    public override void AddTime(float timeUnits, ref PlayContext playContext)
    {
    }
}