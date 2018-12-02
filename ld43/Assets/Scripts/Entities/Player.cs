using UnityEngine;

public class Player : Entity
{
    Camera _camera;
    PlayerConfig _playerConfig;
    
    public float Speed => _stats[StatType.Speed] * (1 + GetEquippedStat(StatType.SpeedPercent) * 0.01f);

    void Awake()
    {
        _camera = GetComponent<Camera>();
        Camera.SetupCurrent(_camera);
    }

    public override void AddTime(float timeUnits, ref PlayContext playContext)
    {
        //Not if there are threats!
        RegenHealth();
    }


    protected override void DoSetup()
    {
        _playerConfig = _config as PlayerConfig;
        _stats.Add(StatType.Speed, _playerConfig.Speed);
    }
}
