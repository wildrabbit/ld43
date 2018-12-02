﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using URandom = UnityEngine.Random;

public enum GameResult
{
    Running,
    Lost,
    Won // Not happening yet :D
}
public enum PlayContext
{
    Action,
    ActionTargetting,
    MenuInteraction,
    Simulating // AI is taking over
}

public class GameController : MonoBehaviour, IEntityController
{

    [SerializeField] GameConfig _gameConfig;

    Map _map;
    Player _player;

    float _timeScale;
    float _elapsedUnits;
    int _turns;

    List<IScheduledEntity> _scheduledEntities;
    List<IScheduledEntity> _scheduledToAdd;

    List<Monster> _monstersToRemove;
    private MessageQueue _messageQueue;
    GameInput _gameInput;
    PlayContext _playContext;

    Dictionary<PlayContext, IPlayContext> _playContexts;

    List<Monster> _monsters;

   
    static DistanceFunctionDelegate _distanceFunction;

    public int MonsterLimit => _gameConfig.MonsterLimit;

    public bool ReachedMonsterLimit => _monsters.Count == MonsterLimit;

    GameResult _gameResult;

    public event Action<GameResult> GameFinished;

    private void Awake()
    {
        _gameInput = new GameInput(_gameConfig.InputDelay);
        _playContexts = new Dictionary<PlayContext, IPlayContext>();
        _playContexts[PlayContext.Action] = new ActionPlayContext();
        _scheduledEntities = new List<IScheduledEntity>();
        _scheduledToAdd = new List<IScheduledEntity>();
        _monsters = new List<Monster>();
        _monstersToRemove = new List<Monster>();
        _messageQueue = new MessageQueue(_gameConfig.QueueLimit);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        _distanceFunction = (_gameConfig.DistanceStrategy == DistanceStrategy.Manhattan) ? (DistanceFunctionDelegate)MapUtils.GetManhattanDistance : (DistanceFunctionDelegate)MapUtils.GetChebyshevDistance;            

        if(_gameConfig.Seed >= 0)
        {
            URandom.InitState(_gameConfig.Seed);
        }
        _map = Instantiate<Map>(_gameConfig.MapPrefab);
        _map.Setup(this);
        _scheduledEntities.Add(_map);

        _player = Instantiate<Player>((Player)(_gameConfig.PlayerConfig.Prefab));
        _player.Setup(_gameConfig.PlayerConfig, _map, this, _messageQueue);
        _player.StartGame(_map.PlayerStartCoords);
        _scheduledEntities.Add(_player);
        _messageQueue.AddEntry("You've finally entered the tower. An ominous silence permeates the hall.");

        _playContext = PlayContext.Action;

        _turns = 0;
        _elapsedUnits = 0.0f;

        _gameResult = GameResult.Running;
    }

    void ClearGame()
    {
        _messageQueue.Clear();
        _scheduledEntities.Clear();
        _scheduledToAdd.Clear();
        _monstersToRemove.Clear();
        if(_player) Destroy(_player.gameObject);
        if(_map) Destroy(_map.gameObject);
        foreach(var monster in _monsters)
        {
            Destroy(monster.gameObject);
        }
        _monsters.Clear();
    }

    IEnumerator RestartGame()
    {
        ClearGame();
        yield return new WaitForSeconds(0.1f);
        StartGame(); // TODO: Change level or smthing.
    }

    // Update is called once per frame
    void Update()
    {
        if(_gameResult != GameResult.Running)
        {
            if(Input.anyKeyDown)
            {
                StartCoroutine(RestartGame());
            }
            return;
        }

        _gameInput.Read();

        bool willSpendTime;
        _playContext = _playContexts[_playContext].Update(_gameInput, _player, _map, _gameConfig, this, _messageQueue, out willSpendTime);

        if(willSpendTime)
        {
            float units = _gameConfig.DefaultTimescale * (1/_player.Speed);
            foreach(var scheduled in _scheduledEntities)
            {
                scheduled.AddTime(units, ref _playContext);
            }
            _elapsedUnits += units;
            _turns++;
            Debug.Log($"Game time: {_elapsedUnits}, turns: {_turns}");

            foreach(var toRemove in _monstersToRemove)
            {
                _scheduledEntities.Remove(toRemove);
                Destroy(toRemove.gameObject);
                _monsters.Remove(toRemove);
            }
            _monstersToRemove.Clear();

            foreach (var toAdd in _scheduledToAdd)
            {
                _scheduledEntities.Add(toAdd);
            }
            _scheduledToAdd.Clear();
        }

        if(Mathf.Approximately(_player.HP,0.0f))
        {
            Destroy(_player.gameObject);
            _player = null;
            _scheduledEntities.Remove(_player);
            _gameResult = GameResult.Lost;
            GameFinished?.Invoke(_gameResult);
        }
    }

    public void CreateMonster(MonsterConfig cfg, Vector2Int coords)
    {
        Monster m = Instantiate<Monster>((Monster)(cfg.Prefab));
        m.name = cfg.Name;
        m.Setup(cfg, _map, this, _messageQueue);
        m.StartGame(coords);
        _scheduledToAdd.Add(m);
        _messageQueue.AddEntry("A new " + m.Name + " has appeared @" + coords.ToString());
        _monsters.Add(m);
    }

    public bool FindEntityNearby(Vector2Int coords, int radius, Entity refEntity = null)
    {
        if (_distanceFunction(_player.Coords, coords) <= radius && refEntity != _player)
            return true;

        foreach(var m in _monsters)
        {
            if(_distanceFunction(m.Coords, coords) <= radius && refEntity != m)
            {
                return true;
            }
        }
        return false;
    }

    public List<Entity> GetEntitiesAt(Vector2Int actionTargetCoords, Entity[] excluded = null)
    {
        List<Entity> entities = new List<Entity>();
        Func<Entity,Vector2Int,Entity[], bool> presenceCheck = (entity, coords, blacklist) => entity.Coords == coords && (blacklist == null || blacklist.Length == 0 || !System.Array.Exists(blacklist, x => x == entity));
        if(presenceCheck(_player,actionTargetCoords,excluded))
        {
            entities.Add(_player);
        }

        foreach(var m in _monsters)
        {
            if(presenceCheck(m, actionTargetCoords, excluded))
            {
                entities.Add(m);
            }
        }

        entities.Sort((x, y) => x.InteractionPriority.CompareTo(y.InteractionPriority));
        return entities;
    }

    public void RemoveEntity(Entity e)
    {
        if(e is Player)
        {
            // Oops
        }
        else
        {
            Monster m = e as Monster;
            if(m != null && _monsters.Contains(m))
            {
                _monstersToRemove.Add(m);
            }
        }
    }

    public Player GetPlayer()
    {
        return _player;
    }

    public DistanceFunctionDelegate DistanceFunction => _distanceFunction;
}
