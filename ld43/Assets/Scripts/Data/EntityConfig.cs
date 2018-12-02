using System;
using System.Collections.Generic;
using UnityEngine;


public enum InteractionType
{
    None,
    Block, // Like bumping vs a wall
    Inspect,
    Pick,
    Altar,
    Attack,
    CustomInteraction
}

public class EntityConfig : ScriptableObject
{
    public Entity Prefab;
    public string Name;
    public StatsConfig Stats;
    public InteractionType DefaultPlayerInteraction;
    public InteractionType MonsterInteraction;
    public int Prio;
}