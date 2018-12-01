using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName ="GameConfig", menuName="Sacrificelike/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Main game prefabs")]
    public Map MapPrefab;

    [Header("Other config")]
    public PlayerConfig PlayerConfig;

    [Header("Configuration parameters")]
    public float DefaultTimescale = 1.0f;
    public bool AllowDiagonals = true;
    public bool BumpingWallsWillSpendMoves = false;
    public float InputDelay = 0.4f;
}
