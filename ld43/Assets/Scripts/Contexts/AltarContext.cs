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

        if(input.idleTurn)
        {
            willSpendTime = true;
            altarContext.uiController.HideAltarView();
            return PlayContext.Action;
        }
        else
        {
            int firstPressed = -1;
            for(int i = 0; i < input.numbersPressed.Length; ++i)
            {
                if(input.numbersPressed[i])
                {
                    firstPressed = i;
                    break;
                }
            }
            if(firstPressed != -1)
            {
                altarContext.uiController.GetAltarView().Select(firstPressed);
            }
        }
        return PlayContext.AltarContext;
    }

}
