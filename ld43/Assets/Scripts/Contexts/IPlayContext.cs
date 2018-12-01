using UnityEngine;
using UnityEditor;

public interface IPlayContext
{
    PlayContext Update(GameInput input, Player player, Map map, GameConfig config, out bool willSpendTime);
}