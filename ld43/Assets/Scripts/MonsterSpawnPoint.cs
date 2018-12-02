using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint: MonoBehaviour
{
    [Serializable]
    public class MonsterSpawnEntry
    {
        public MonsterConfig Config;
        public int weight;
    }

    public List<MonsterSpawnEntry> MonsterEntries;
    public float MinSpawnTime;
    public float MaxSpawnTime;
}