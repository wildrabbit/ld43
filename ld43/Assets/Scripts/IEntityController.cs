using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityController
{
    void CreateMonster(MonsterConfig cfg, Vector2Int coords);

    bool FindEntityNearby(Vector2Int coords, int radius);
}
