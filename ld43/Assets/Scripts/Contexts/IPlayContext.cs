using UnityEngine;
using UnityEditor;

public interface IPlayContext
{
    PlayContext Update(BaseContextData context, out bool willSpendTime);
}