using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    public float _elapsedNextAction;
    public float _decisionDelay;

    MonsterConfig _monsterConfig;

    protected override void DoSetup()
    {
        _monsterConfig = _config as MonsterConfig;
        _decisionDelay = _monsterConfig.ThinkingDelay;
        _elapsedNextAction = 0.0f;
    }

    public override void AddTime(float timeUnits, ref PlayContext playContext)
    {
        _elapsedNextAction += timeUnits;
        if(_elapsedNextAction >= _decisionDelay)
        {
            Player p = _entityController.GetPlayer();
            int distance =_entityController.DistanceFunction(p.Coords, this.Coords);
            
            // TODO: Allow for different AIs
            if(distance <= _monsterConfig.Stats.Range)
            {
                // TODO: Add timed projectile logic

                ResultData result;
                BattleUtils.SolveAttack(this, p, out result);
                ProcessResultMessages(result);
            }
            //else if(distance <= _monsterConfig.Stats.VisibilityRange)
            //{
            //    // Pathfinding!
            //}
            else
            {
                // TODO: Alter movement pattern
                int[] xDelta = new int[] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
                int[] yDelta = new int[] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
                List<Vector2Int> candidateCoords = new List<Vector2Int>();
                for(int i = 0; i < 9; ++i)
                {
                    Vector2Int coord = new Vector2Int(Coords.x + xDelta[i], Coords.y + yDelta[i]);
                    if (_map.IsWalkable(coord) && !_entityController.FindEntityNearby(coord, 0, this))
                    {
                        candidateCoords.Add(coord);
                    }
                }
                if(candidateCoords.Count > 0)
                {
                    SetGridPos(candidateCoords[Random.Range(0, candidateCoords.Count)]);
                }
            }

            _elapsedNextAction = 0.0f;
            _decisionDelay = _monsterConfig.ThinkingDelay;
            // AI time!
            // If player is in range, then either attack or escape.
            // If not, but is visible, chase
            // If not, move around like an idiot (should we cull non-visible updates?)
        }
    }

    void ProcessResultMessages(ResultData result)
    {
        if(result.AttackerFlopped)
        {
            _messageQueue.AddEntry(Name + " missed " + result.DefenderName + " by a hair!");
        }
        if(result.DefenderAvoided)
        {
            _messageQueue.AddEntry(result.DefenderName + " skillfully dodged " + Name + "'s attack!");
        }
        if(result.Critical)
        {
            _messageQueue.AddEntry("Critical hit!");
        }
        if(result.AttackerDmgInflicted > 0)
        {
            string dmgString = Name + " hit " + result.DefenderName + " for " + (int)result.AttackerDmgInflicted + " HP";
            if(result.DefenderDefeated)
            {
                dmgString += ". Killing blow!";
            }
            else
            {
                dmgString += ". Remaining: " + ((int)result.DefenderHP).ToString();
            }
            _messageQueue.AddEntry($"<color=red>{dmgString}</color>");
        }
    }
}
