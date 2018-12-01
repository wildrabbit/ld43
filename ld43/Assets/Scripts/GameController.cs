using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEntry
{
    float _last;

    string _buttonKey;
    float _delay;

    public InputEntry(string key, float delay)
    {
        _buttonKey = key;
        _delay = delay;
        _last = -1;
    }

    public bool Read()
    {
        if((_last < 0 || Time.time - _last >= _delay) && Input.GetButton(_buttonKey))
        {
            _last = Time.time;
            return true;
        }
        return false;
    }
}


public class GameInput
{
    float _moveInputDelay;

    public int xAxis;
    public int yAxis;
    public bool idleTurn;

    InputEntry leftInput;
    InputEntry rightInput;
    InputEntry upInput;
    InputEntry downInput;
    InputEntry idleInput;

    public GameInput(float inputDelay)
    {
        _moveInputDelay = inputDelay;
        leftInput = new InputEntry("left", _moveInputDelay);
        rightInput = new InputEntry("right", _moveInputDelay);
        upInput = new InputEntry("up", _moveInputDelay);
        downInput = new InputEntry("down", _moveInputDelay);
        idleInput = new InputEntry("idle", _moveInputDelay);
    }

    public void Read()
    {
        xAxis = leftInput.Read() ? -1 : rightInput.Read() ? 1 : 0;
        yAxis = downInput.Read() ? -1 : upInput.Read() ? 1 : 0;
        idleTurn = idleInput.Read();
    }
}

public enum PlayContext
{
    Action,
    ActionTargetting,
    MenuInteraction,
    Simulating // AI is taking over
}

public class GameController : MonoBehaviour
{
    [SerializeField] GameConfig _gameConfig;

    Map _map;
    Player _player;

    float _timeScale;
    float _elapsedUnits;
    int _turns;

    List<IScheduledEntity> _scheduledEntities;

    GameInput _gameInput;
    PlayContext _playContext;

    Dictionary<PlayContext, IPlayContext> _playContexts;


    private void Awake()
    {
        _gameInput = new GameInput(_gameConfig.InputDelay);
        _playContexts = new Dictionary<PlayContext, IPlayContext>();
        _playContexts[PlayContext.Action] = new ActionPlayContext();
        _scheduledEntities = new List<IScheduledEntity>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        ClearGame();
        _map = Instantiate<Map>(_gameConfig.MapPrefab);
        //_map.Setup();
        _player = Instantiate<Player>(_gameConfig.PlayerConfig.PlayerPrefab);
        _player.Setup(_gameConfig.PlayerConfig, _map);
        _player.StartGame();

        _playContext = PlayContext.Action;

        _turns = 0;
        _elapsedUnits = 0.0f;
    }

    void ClearGame()
    {
        _scheduledEntities.Clear();
        if(_player) Destroy(_player.gameObject);
        if(_map) Destroy(_map.gameObject);
    }

    void RestartGame()
    {
        StartGame(); // TODO: Change level or smthing.
    }

    // Update is called once per frame
    void Update()
    {
        _gameInput.Read();

        bool willSpendTime;
        _playContext = _playContexts[_playContext].Update(_gameInput, _player, _map, _gameConfig, out willSpendTime);

        if(willSpendTime)
        {
            float units = _gameConfig.DefaultTimescale;
            foreach(var scheduled in _scheduledEntities)
            {
                scheduled.AddTime(units, ref _playContext);
            }
            _elapsedUnits += units;
            _turns++;
        }
    }
}
