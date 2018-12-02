using UnityEngine;
using System.Collections;

public class AltarContext : IPlayContext
{
    public PlayContext Update(BaseContextData context, out bool willSpendTime)
    {
        GameInput input = context.input;
        AltarContextData altarContext = context as AltarContextData;
        Player player = altarContext.player;
        GameConfig config = altarContext.config;
        AltarConfig altarConfig = altarContext.altarConfig;
        MessageQueue queue = altarContext.queue;
       
        willSpendTime = false; // Only when it closes
        return PlayContext.AltarContext;
    }

}
