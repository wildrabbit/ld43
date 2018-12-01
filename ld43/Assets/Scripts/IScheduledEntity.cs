using UnityEngine;
using System.Collections;

public interface IScheduledEntity
{
    float Speed { get; }
    void AddTime(float timeUnits, ref PlayContext playContext);
}
