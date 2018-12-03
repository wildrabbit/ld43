using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ResultData
{
    public float AttackerDmgInflicted;
    public float DefenderDmgTaken;
    public bool AttackerFlopped;
    public bool DefenderAvoided;
    public bool Critical;
    public bool DefenderDefeated;
    public float DefenderHP;
    public string DefenderName;
}

public class ActionPlayContext : IPlayContext
{
    public PlayContext Update(BaseContextData context, out bool willSpendTime)
    {
        ActionContextData actionData = context as ActionContextData;
        GameInput input = actionData.input;
        Player player = actionData.player;
        Map map = actionData.map;
        GameConfig config = actionData.config;
        IEntityController entityController = actionData.controller;
        MessageQueue queue = actionData.queue;

        willSpendTime = false;

        if(input.xAxis != 0 || input.yAxis != 0)
        {
            Vector2Int actionTargetCoords = player.Coords;
            actionTargetCoords.x += input.xAxis;
            actionTargetCoords.y += input.yAxis;

            if (map.IsWalkable(actionTargetCoords))
            {
                List<Entity> entities = entityController.GetEntitiesAt(actionTargetCoords, new Entity[] { player });

                bool playerWillMove = true;
                bool stop = false;

                for(int i = 0; i < entities.Count; ++i)
                {
                    var e = entities[i];
                    switch(e.DefaultInteraction)
                    {
                        case InteractionType.None:
                        {
                            break;
                        }
                        case InteractionType.Attack:
                        {
                            stop = true;
                            ResultData resultData;
                            SolveAttack(player, e, out resultData);
                            ProcessResultMessages(resultData, queue);

                            if (resultData.DefenderDefeated)
                            {
                                entityController.RemoveEntity(e);
                                int j = i + 1;
                                for (; j < entities.Count; ++j)
                                {
                                    if(entities[j].DefaultInteraction == InteractionType.Attack || entities[j].DefaultInteraction == InteractionType.CustomInteraction)
                                    {
                                        playerWillMove = false;
                                        break;
                                    }
                                }
                                if(j == entities.Count)
                                {
                                    stop = false;
                                }
                            }
                            else
                            {
                                playerWillMove = false;
                            }
                            break;
                        }
                        case InteractionType.CustomInteraction:
                        {
                            return e.PlayerInteracts();                            
                        }
                    }
                    if(stop)
                    {
                        break;
                    }
                }

                if(playerWillMove)
                {
                    player.SetGridPos(actionTargetCoords);
                }
                willSpendTime = true;
            }
            else
            {
                // TODO: Check for special wall tiles
                willSpendTime = config.BumpingWallsWillSpendMoves;                
            }
        }
        else if(input.idleTurn)
        {
            willSpendTime = true;
        }

        return PlayContext.Action;
    }

    void SolveAttack(Entity attacker, Entity defender, out ResultData resultData)
    {
        BattleUtils.SolveAttack(attacker, defender, out resultData);
    }

    void ProcessResultMessages(ResultData data, MessageQueue queue)
    {
        if (data.AttackerFlopped)
        {
            queue.AddEntry("You fumbled! Lucky no one from town's here to see you");
        }
        if (data.DefenderAvoided)
        {
            queue.AddEntry("The " + data.DefenderName +" swiftly dodges your attack");
        }
        if(data.AttackerDmgInflicted > 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<color=green>");
            builder.Append("The " + data.DefenderName + " is hit for ").Append((int)data.AttackerDmgInflicted).Append(" HP");
            if (data.DefenderDefeated)
            {
                builder.Append(" and falls down!");
            }
            else builder.Append(". Remaining: ").Append((int)data.DefenderHP);
            builder.Append("</color>");
            queue.AddEntry(builder.ToString());
        }
    }
}
