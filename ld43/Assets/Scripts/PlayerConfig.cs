using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Sacrificelike/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public Player PlayerPrefab;
    public float DefaultSpeed;
    public float MaxHP;
    public float HP;
}
