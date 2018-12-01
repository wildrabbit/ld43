using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionPlayContext : IPlayContext
{
    public PlayContext Update(GameInput input, Player player, Map map, GameConfig config, out bool willSpendTime)
    {
        willSpendTime = false;

        if(input.xAxis != 0 || input.yAxis != 0)
        {
            Vector2Int actionTargetCoords = player.Coords;
            actionTargetCoords.x += input.xAxis;
            actionTargetCoords.y += input.yAxis;

            if (map.IsWalkable(actionTargetCoords))
            {
                player.SetGridPos(actionTargetCoords);
                willSpendTime = true;
            }
            else
            {
                willSpendTime = config.BumpingWallsWillSpendMoves;
                // Is there anything we can interact with?
            }
        }
        else if(input.idleTurn)
        {
            willSpendTime = true;
        }

        return PlayContext.Action;
    }
}
