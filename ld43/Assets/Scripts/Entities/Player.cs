using UnityEngine;

public class Player : Entity
{
    Camera _camera;
    PlayerConfig _playerConfig;
    
    public float Speed => _playerConfig.Speed;

    void Awake()
    {
        _camera = GetComponent<Camera>();
        Camera.SetupCurrent(_camera);
    }

    public override void AddTime(float timeUnits, ref PlayContext playContext)
    {
        if(_hp < _playerConfig.Stats.LifeData.MaxHP)
        {
            _hp = Mathf.Min(_hp + _playerConfig.Stats.LifeData.HPRegen, _playerConfig.Stats.LifeData.MaxHP);
            if(Mathf.Approximately(_hp, _maxHP))
            {
                _messageQueue.AddEntry(Name + "'s HP restored to Max(" + MaxHP + ")");
            }

        }
    }

    protected override void DoSetup()
    {
        _playerConfig = _config as PlayerConfig;
    }
}
