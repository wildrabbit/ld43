using UnityEngine;
using UnityEditor;

public interface IPlayContext
{
    PlayContext Update(GameInput input, Player player, Map map, GameConfig config, IEntityController entityController, MessageQueue queue, out bool willSpendTime);
}