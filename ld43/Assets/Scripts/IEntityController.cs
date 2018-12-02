using System;
using System.Collections.Generic;
using UnityEngine;

public delegate int DistanceFunctionDelegate(Vector2Int p1, Vector2Int p2);

public interface IEntityController
{
    int MonsterLimit { get; }
    bool ReachedMonsterLimit { get; }
    DistanceFunctionDelegate DistanceFunction { get; }

    void CreateMonster(MonsterConfig cfg, Vector2Int coords);

    bool FindEntityNearby(Vector2Int coords, int radius, Entity refEntity = null);
    List<Entity> GetEntitiesAt(Vector2Int actionTargetCoords, Entity[] excluded = null);
    void RemoveEntity(Entity e);
    Player GetPlayer();
}
